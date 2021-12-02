﻿using JetBrains.Annotations;
using LinFx.Extensions.EntityFrameworkCore.DependencyInjection;
using System;
using System.Collections.Generic;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public class EfCoreDbContextOptions
    {
        internal List<Action<DbContextConfigurationContext>> DefaultPreConfigureActions { get; } = new List<Action<DbContextConfigurationContext>>();

        internal Action<DbContextConfigurationContext> DefaultConfigureAction { get; set; }

        internal Dictionary<Type, List<object>> PreConfigureActions { get; } = new Dictionary<Type, List<object>>();

        internal Dictionary<Type, object> ConfigureActions { get; } = new Dictionary<Type, object>();

        internal Dictionary<Type, Type> DbContextReplacements { get; } = new Dictionary<Type, Type>();

        public void PreConfigure([NotNull] Action<DbContextConfigurationContext> action)
        {
            Check.NotNull(action, nameof(action));

            DefaultPreConfigureActions.Add(action);
        }

        public void Configure([NotNull] Action<DbContextConfigurationContext> action)
        {
            Check.NotNull(action, nameof(action));

            DefaultConfigureAction = action;
        }

        public bool IsConfiguredDefault()
        {
            return DefaultConfigureAction != null;
        }

        public void PreConfigure<TDbContext>([NotNull] Action<DbContextConfigurationContext<TDbContext>> action)
            where TDbContext : EfCoreDbContext
        {
            Check.NotNull(action, nameof(action));

            var actions = PreConfigureActions.GetOrDefault(typeof(TDbContext));
            if (actions == null)
            {
                PreConfigureActions[typeof(TDbContext)] = actions = new List<object>();
            }

            actions.Add(action);
        }

        public void Configure<TDbContext>([NotNull] Action<DbContextConfigurationContext<TDbContext>> action)
            where TDbContext : EfCoreDbContext
        {
            Check.NotNull(action, nameof(action));

            ConfigureActions[typeof(TDbContext)] = action;
        }

        public bool IsConfigured<TDbContext>()
        {
            return IsConfigured(typeof(TDbContext));
        }

        public bool IsConfigured(Type dbContextType)
        {
            return ConfigureActions.ContainsKey(dbContextType);
        }

        internal Type GetReplacedTypeOrSelf(Type dbContextType)
        {
            var replacementType = dbContextType;
            while (true)
            {
                if (DbContextReplacements.TryGetValue(replacementType, out var foundType))
                {
                    if (foundType == dbContextType)
                        throw new Exception("Circular DbContext replacement found for " + dbContextType.AssemblyQualifiedName);

                    replacementType = foundType;
                }
                else
                {
                    return replacementType;
                }
            }
        }
    }
}
﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public class PermissionDefinitionManager : IPermissionDefinitionManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;
        private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;
        protected PermissionOptions Options { get; }

        public PermissionDefinitionManager(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider)
        {
            Options = options.Value;
            _serviceProvider = serviceProvider;

            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, true);
            _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(CreatePermissionGroupDefinitions, true);
        }

        /// <summary>
        /// 权限
        /// </summary>
        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;

        /// <summary>
        /// 权限组
        /// </summary>
        protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;

        /// <summary>
        /// 创建权限定义
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var groupDefinition in PermissionGroupDefinitions.Values)
            {
                foreach (var permission in groupDefinition.Permissions)
                {
                    AddPermissionToDictionaryRecursively(permissions, permission);
                }
            }

            return permissions;
        }

        /// <summary>
        /// 创建权限组定义
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = new PermissionDefinitionContext();
            
            Options
                .DefinitionProviders
                .Select(p => scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                .ToList()
                .ForEach(item => item.Define(context));

            return context.Groups;
        }

        public virtual PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
                throw new LinFxException("Undefined permission: " + name);

            return permission;
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return PermissionDefinitions.GetOrDefault(name);
        }

        public virtual IReadOnlyList<PermissionDefinition> GetPermissions()
        {
            return PermissionDefinitions.Values.ToImmutableList();
        }

        public IReadOnlyList<PermissionGroupDefinition> GetGroups()
        {
            return PermissionGroupDefinitions.Values.ToImmutableList();
        }

        protected virtual void AddPermissionToDictionaryRecursively(Dictionary<string, PermissionDefinition> permissions, PermissionDefinition permission)
        {
            if (permissions.ContainsKey(permission.Name))
                throw new LinFxException("Duplicate permission name: " + permission.Name);

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }
    }
}
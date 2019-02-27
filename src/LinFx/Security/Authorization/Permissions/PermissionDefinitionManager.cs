using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace LinFx.Security.Authorization.Permissions
{
    public class PermissionDefinitionManager : IPermissionDefinitionManager
    {
        private readonly Lazy<List<IPermissionDefinitionProvider>> _lazyProviders;
        protected List<IPermissionDefinitionProvider> Providers => _lazyProviders.Value;

        private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;
        protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;

        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;
        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;

        protected PermissionOptions Options { get; }
        private readonly IServiceProvider _serviceProvider;

        public PermissionDefinitionManager(
            IOptions<PermissionOptions> options,
            IServiceProvider serviceProvider)
        {
            Options = options.Value;
            _serviceProvider = serviceProvider;

            _lazyProviders = new Lazy<List<IPermissionDefinitionProvider>>(CreatePermissionProviders, true);
            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(CreatePermissionDefinitions, true);
            _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(CreatePermissionGroupDefinitions, true);
        }

        protected virtual List<IPermissionDefinitionProvider> CreatePermissionProviders()
        {
            foreach (var p in Options.DefinitionProviders)
            {
                var t = _serviceProvider.GetRequiredService(p);
            }

            return Options
                .DefinitionProviders
                .Select(p => _serviceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                .ToList();
        }

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

        protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
        {
            var context = new PermissionDefinitionContext();

            foreach (var provider in Providers)
            {
                provider.Define(context);
            }

            return context.Groups;
        }

        public virtual PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new LinFxException("Undefined permission: " + name);
            }

            return permission;
        }

        public virtual PermissionDefinition GetOrNull(string name)
        {
            Check.NotNull(name, nameof(name));

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
            {
                throw new LinFxException("Duplicate permission name: " + permission.Name);
            }

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }
    }
}
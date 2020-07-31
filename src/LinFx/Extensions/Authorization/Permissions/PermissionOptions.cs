using LinFx.Collections;

namespace LinFx.Extensions.Authorization.Permissions
{
    /// <summary>
    /// 权限选项
    /// </summary>
    public class PermissionOptions
    {
        /// <summary>
        /// 权限定义提供者
        /// </summary>
        public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

        /// <summary>
        /// 权限值提供者
        /// </summary>
        public ITypeList<IPermissionValueProvider> ValueProviders { get; }

        public PermissionOptions()
        {
            DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
            ValueProviders = new TypeList<IPermissionValueProvider>();
        }
    }
}
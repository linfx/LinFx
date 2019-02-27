using System.Threading.Tasks;

namespace LinFx.Security.Authorization.Permissions
{
    public interface IPermissionValueProvider
    {
        string Name { get; }

        Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context);
    }
}
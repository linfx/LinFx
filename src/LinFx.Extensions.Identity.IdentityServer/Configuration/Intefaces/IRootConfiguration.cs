namespace LinFx.Extensions.Identity.IdentityServer.Configuration.Intefaces
{
    public interface IRootConfiguration
    {
        IAdminConfiguration AdminConfiguration { get; }

        IRegisterConfiguration RegisterConfiguration { get; }
    }
}
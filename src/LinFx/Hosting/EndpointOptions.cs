namespace LinFx.Hosting
{
    public class EndpointsOptions
    {
        public virtual bool IsEndpointEnabled(Endpoint endpoint)
        {
            return true;
        }
    }
}

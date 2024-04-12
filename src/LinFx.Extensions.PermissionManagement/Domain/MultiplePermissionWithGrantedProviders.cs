namespace LinFx.Extensions.PermissionManagement;

public class MultiplePermissionWithGrantedProviders
{
    public List<PermissionWithGrantedProviders> Result { get; } = [];

    public MultiplePermissionWithGrantedProviders() { }

    public MultiplePermissionWithGrantedProviders(string[] names)
    {
        foreach (var name in names)
        {
            Result.Add(new PermissionWithGrantedProviders(name, false));
        }
    }
}

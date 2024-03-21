using LinFx.Utils;

namespace LinFx.Extensions.PermissionManagement;

public class MultiplePermissionWithGrantedProviders
{
    public List<PermissionWithGrantedProviders> Result { get; }

    public MultiplePermissionWithGrantedProviders()
    {
        Result = [];
    }

    public MultiplePermissionWithGrantedProviders(string[] names)
    {
        Check.NotNull(names, nameof(names));

        Result = [];

        foreach (var name in names)
        {
            Result.Add(new PermissionWithGrantedProviders(name, false));
        }
    }
}

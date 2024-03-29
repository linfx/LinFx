namespace LinFx.Extensions.Authorization.Permissions;

public class MultiplePermissionGrantResult
{
    public bool AllGranted => Result.Values.All(x => x == PermissionGrantResult.Granted);

    public bool AllProhibited => Result.Values.All(x => x == PermissionGrantResult.Prohibited);

    public Dictionary<string, PermissionGrantResult> Result { get; }

    public MultiplePermissionGrantResult()
    {
        Result = [];
    }

    public MultiplePermissionGrantResult(string[] names, PermissionGrantResult grantResult = PermissionGrantResult.Undefined)
    {
        Check.NotNull(names, nameof(names));

        Result = [];

        foreach (var name in names)
        {
            Result.Add(name, grantResult);
        }
    }
}

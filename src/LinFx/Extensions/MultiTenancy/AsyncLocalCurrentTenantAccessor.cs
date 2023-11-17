namespace LinFx.Extensions.MultiTenancy;

public class AsyncLocalCurrentTenantAccessor : ICurrentTenantAccessor
{
    public static AsyncLocalCurrentTenantAccessor Instance { get; } = new();

    public TenantInfo Current
    {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<TenantInfo> _currentScope;

    private AsyncLocalCurrentTenantAccessor()
    {
        _currentScope = new AsyncLocal<TenantInfo>();
    }
}

using System;

namespace LinFx.Extensions.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        string Id { get; }

        string Name { get; }

        IDisposable Change(string id, string name = default);
    }
}
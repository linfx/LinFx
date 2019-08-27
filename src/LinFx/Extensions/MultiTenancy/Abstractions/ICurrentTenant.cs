using System;

namespace LinFx.Extensions.MultiTenancy
{
    public interface ICurrentTenant
    {
        bool IsAvailable { get; }

        [CanBeNull]
        string Id { get; }

        [CanBeNull]
        string Name { get; }

        IDisposable Change(string id, string name = default);
    }
}
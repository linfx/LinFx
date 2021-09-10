using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Auditing
{
    public interface IAuditLogSaveHandle : IDisposable
    {
        Task SaveAsync();
    }
}
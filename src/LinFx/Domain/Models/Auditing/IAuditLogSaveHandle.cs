using System;
using System.Threading.Tasks;

namespace LinFx.Domain.Models.Auditing
{
    public interface IAuditLogSaveHandle : IDisposable
    {
        void Save();

        Task SaveAsync();
    }
}
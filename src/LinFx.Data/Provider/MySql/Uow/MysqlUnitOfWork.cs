using LinFx.Domain.Uow;
using System;
using System.Threading.Tasks;

namespace LinFx.Data.Provider.Uow
{
    public class MySqlUnitOfWork : UnitOfWorkBase
    {
        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        protected override void BeginUow()
        {
            throw new NotImplementedException();
        }

        protected override void CompleteUow()
        {
            throw new NotImplementedException();
        }

        protected override Task CompleteUowAsync()
        {
            throw new NotImplementedException();
        }

        protected override void DisposeUow()
        {
            throw new NotImplementedException();
        }
    }
}

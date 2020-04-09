using Microsoft.EntityFrameworkCore;

namespace LinFx.Data.Abstractions
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}

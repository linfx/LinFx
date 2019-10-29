using Microsoft.EntityFrameworkCore;

namespace LinFx.EntityFrameworkCore
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}

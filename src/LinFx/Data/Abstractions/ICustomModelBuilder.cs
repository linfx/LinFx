using Microsoft.EntityFrameworkCore;

namespace LinFx.Data
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}

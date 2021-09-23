using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public interface ICustomModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}

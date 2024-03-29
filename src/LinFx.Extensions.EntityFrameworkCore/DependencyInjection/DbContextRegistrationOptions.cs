using LinFx.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection;

/// <summary>
/// ���ݿ�ע�����ö���
/// </summary>
public class DbContextRegistrationOptions : CommonDbContextRegistrationOptions, IDbContextRegistrationOptionsBuilder
{
    public Dictionary<Type, object> EntityOptions { get; }

    public DbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
        : base(originalDbContextType, services)
    {
        EntityOptions = new Dictionary<Type, object>();
    }

    public void Entity<TEntity>(Action<EntityOptions<TEntity>> optionsAction) where TEntity : IEntity
    {
        Services.Configure<EntityOptions>(options =>
        {
            options.Entity(optionsAction);
        });
    }
}

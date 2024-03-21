using LinFx;
using LinFx.Domain.Entities;
using LinFx.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// This is a base class for dbcoUse derived
/// </summary>
public abstract class CommonDbContextRegistrationOptions : ICommonDbContextRegistrationOptionsBuilder
{
    public Type OriginalDbContextType { get; }

    public IServiceCollection Services { get; }

    public Dictionary<Type, Type> ReplacedDbContextTypes { get; }

    public Type DefaultRepositoryDbContextType { get; protected set; }

    public Type DefaultRepositoryImplementationType { get; private set; }

    public Type DefaultRepositoryImplementationTypeWithoutKey { get; private set; }

    public bool RegisterDefaultRepositories { get; private set; }

    public bool IncludeAllEntitiesForDefaultRepositories { get; private set; }

    public Dictionary<Type, Type> CustomRepositories { get; }

    public List<Type> SpecifiedDefaultRepositories { get; }

    public bool SpecifiedDefaultRepositoryTypes => DefaultRepositoryImplementationType != null && DefaultRepositoryImplementationTypeWithoutKey != null;

    protected CommonDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services)
    {
        OriginalDbContextType = originalDbContextType;
        Services = services;
        DefaultRepositoryDbContextType = originalDbContextType;
        CustomRepositories = [];
        ReplacedDbContextTypes = [];
        SpecifiedDefaultRepositories = [];
    }

    public ICommonDbContextRegistrationOptionsBuilder ReplaceDbContext<TOtherDbContext>() => ReplaceDbContext(typeof(TOtherDbContext));

    public ICommonDbContextRegistrationOptionsBuilder ReplaceDbContext<TOtherDbContext, TTargetDbContext>() => ReplaceDbContext(typeof(TOtherDbContext), typeof(TTargetDbContext));

    public ICommonDbContextRegistrationOptionsBuilder ReplaceDbContext(Type otherDbContextType, Type? targetDbContextType = null)
    {
        if (!otherDbContextType.IsAssignableFrom(OriginalDbContextType))
            throw new Exception($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {otherDbContextType.AssemblyQualifiedName}!");

        ReplacedDbContextTypes[otherDbContextType] = targetDbContextType;

        return this;
    }

    public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(bool includeAllEntities = false)
    {
        RegisterDefaultRepositories = true;
        IncludeAllEntitiesForDefaultRepositories = includeAllEntities;

        return this;
    }

    public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType, bool includeAllEntities = false)
    {
        if (!defaultRepositoryDbContextType.IsAssignableFrom(OriginalDbContextType))
            throw new Exception($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {defaultRepositoryDbContextType.AssemblyQualifiedName}!");

        DefaultRepositoryDbContextType = defaultRepositoryDbContextType;

        return AddDefaultRepositories(includeAllEntities);
    }

    public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false) => AddDefaultRepositories(typeof(TDefaultRepositoryDbContext), includeAllEntities);

    public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepository<TEntity>() => AddDefaultRepository(typeof(TEntity));

    public ICommonDbContextRegistrationOptionsBuilder AddDefaultRepository(Type entityType)
    {
        EntityHelper.CheckEntity(entityType);

        SpecifiedDefaultRepositories.AddIfNotContains(entityType);

        return this;
    }

    public ICommonDbContextRegistrationOptionsBuilder AddRepository<TEntity, TRepository>()
    {
        AddCustomRepository(typeof(TEntity), typeof(TRepository));

        return this;
    }

    public ICommonDbContextRegistrationOptionsBuilder SetDefaultRepositoryClasses(Type repositoryImplementationType, Type repositoryImplementationTypeWithoutKey)
    {
        Check.NotNull(repositoryImplementationType, nameof(repositoryImplementationType));
        Check.NotNull(repositoryImplementationTypeWithoutKey, nameof(repositoryImplementationTypeWithoutKey));

        DefaultRepositoryImplementationType = repositoryImplementationType;
        DefaultRepositoryImplementationTypeWithoutKey = repositoryImplementationTypeWithoutKey;

        return this;
    }

    private void AddCustomRepository(Type entityType, Type repositoryType)
    {
        if (!typeof(IEntity).IsAssignableFrom(entityType))
            throw new Exception($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");

        if (!typeof(IRepository).IsAssignableFrom(repositoryType))
            throw new Exception($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IBasicRepository<>).AssemblyQualifiedName}.");

        CustomRepositories[entityType] = repositoryType;
    }
}

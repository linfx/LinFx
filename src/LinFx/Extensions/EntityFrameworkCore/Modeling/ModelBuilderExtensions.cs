using LinFx.Extensions.EntityFrameworkCore;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    private const string ModelDatabaseProviderAnnotationKey = "_DatabaseProvider";
    //private const string ModelMultiTenancySideAnnotationKey = "_MultiTenancySide";

    #region MultiTenancySide

    //public static void SetMultiTenancySide(
    //    this ModelBuilder modelBuilder,
    //    MultiTenancySides side)
    //{
    //    modelBuilder.Model.SetAnnotation(ModelMultiTenancySideAnnotationKey, side);
    //}

    //public static MultiTenancySides GetMultiTenancySide(this ModelBuilder modelBuilder)
    //{
    //    var value = modelBuilder.Model[ModelMultiTenancySideAnnotationKey];
    //    if (value == null)
    //    {
    //        return MultiTenancySides.Both;
    //    }

    //    return (MultiTenancySides) value;
    //}

    ///// <summary>
    ///// Returns true if this is a database schema that is used by the host
    ///// but can also be shared with the tenants.
    ///// </summary>
    //public static bool IsHostDatabase(this ModelBuilder modelBuilder)
    //{
    //    return modelBuilder.GetMultiTenancySide().HasFlag(MultiTenancySides.Host);
    //}

    ///// <summary>
    ///// Returns true if this is a database schema that is used by the tenants
    ///// but can also be shared with the host.
    ///// </summary>
    //public static bool IsTenantDatabase(this ModelBuilder modelBuilder)
    //{
    //    return modelBuilder.GetMultiTenancySide().HasFlag(MultiTenancySides.Tenant);
    //}

    ///// <summary>
    ///// Returns true if this is a database schema that is only used by the host
    ///// and should not contain tenant-only tables.
    ///// </summary>
    //public static bool IsHostOnlyDatabase(this ModelBuilder modelBuilder)
    //{
    //    return modelBuilder.GetMultiTenancySide() == MultiTenancySides.Host;
    //}

    ///// <summary>
    ///// Returns true if this is a database schema that is only used by tenants.
    ///// and should not contain host-only tables.
    ///// </summary>
    //public static bool IsTenantOnlyDatabase(this ModelBuilder modelBuilder)
    //{
    //    return modelBuilder.GetMultiTenancySide() == MultiTenancySides.Tenant;
    //}

    #endregion

    #region DatabaseProvider

    public static void SetDatabaseProvider(
        this ModelBuilder modelBuilder,
        EfDatabaseProvider databaseProvider)
    {
        modelBuilder.Model.SetAnnotation(ModelDatabaseProviderAnnotationKey, databaseProvider);
    }

    public static void ClearDatabaseProvider(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.RemoveAnnotation(ModelDatabaseProviderAnnotationKey);
    }

    public static EfDatabaseProvider? GetDatabaseProvider(
        this ModelBuilder modelBuilder
    )
    {
        return (EfDatabaseProvider?)modelBuilder.Model[ModelDatabaseProviderAnnotationKey];
    }

    public static bool IsUsingMySQL(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.MySql;
    }

    public static bool IsUsingOracle(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.Oracle;
    }

    public static bool IsUsingSqlServer(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.SqlServer;
    }

    public static bool IsUsingPostgreSql(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.PostgreSql;
    }

    public static bool IsUsingSqlite(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.Sqlite;
    }

    public static void UseInMemory(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfDatabaseProvider.InMemory);
    }

    public static bool IsUsingInMemory(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.InMemory;
    }

    public static void UseCosmos(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfDatabaseProvider.Cosmos);
    }

    public static bool IsUsingCosmos(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.Cosmos;
    }

    public static void UseFirebird(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfDatabaseProvider.Firebird);
    }

    public static bool IsUsingFirebird(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfDatabaseProvider.Firebird;
    }

    #endregion
}

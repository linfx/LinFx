# LinFx



#数据库使用

public void UsingDbConnectionFactory(Action<DbConnectionFactory> action)
{
    const string connectionString = "server=xx;database=xx;uid=root;pwd=123456;Charset=utf8;";
    using (var factory = new DbConnectionFactory(connectionString, MySqlProvider.Instance))
    {
        action(factory);
    }
}

public void UsingRepository<TEntity>(Action<IRepository<TEntity>> action) where TEntity : class, IEntity<string>
{
    UsingDbConnectionFactory(factory =>
    {
        action(new Repository<TEntity>(factory));
    });
}

EditionManager _editionManager;

public EditionManagerTest()
{
    UsingRepository<Edition>(repository =>
    {
        _editionManager = new EditionManager(repository);
    });
}

[Fact]
public void CreateAsyncTest()
{
    var item = new Edition
    {
        Name = DateTime.Now.ToString(),
    };
    _editionManager.CreateAsync(item);
}

[Fact]
public async Task UpdateAsyncTestAsync()
{
    var item = await _editionManager.GetAsync("0a85e9c1fc944ea68b7860c6a7343ec2");
    Assert.NotNull(item);

    item.Name = "ok";
    await _editionManager.UpdateAsync(item);
}

[Fact]
public async Task GetAsyncTestAsync()
{
    var item = await _editionManager.GetAsync("0a85e9c1fc944ea68b7860c6a7343ec2");
    Assert.NotNull(item);
}
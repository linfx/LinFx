# LinFx
[![](https://img.shields.io/badge/.NET%20Core-2.0.0-brightgreen.svg?style=flat-square)](https://www.microsoft.com/net/download/core) 
[![Build status](https://ci.appveyor.com/api/projects/status/mcwi2kqe0daija6c?svg=true)](https://ci.appveyor.com/project/ElderJames/shriekfx)
[![GitHub license](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](https://github.com/linfx/LinFx/blob/master/LICENSE)

一个基于 .NET Core 2.0 开发的简单易用的快速开发框架，遵循领域驱动设计（DDD）规范约束，提供实现事件驱动、事件回溯、响应式等特性的基础设施。让开发者享受到正真意义的面向对象设计模式来带的美感。

# LinFx.Extensions

Caching、DapperExtensions、Elasticsearch、EventBus、Metrics、Mongo、RabbitMQ

### 特性：

1. 领域驱动设计（DDD）
2. 事件驱动架构 (EDA)
3. 事件回溯 （ES）
4. 最终一致性 （Eventually Consistent）
6. 框架中每个组件都有基础实现，最简单时只需一个核心类库就能跑起来
7. 遵循端口与适配器模式，框架组件适配多种第三方组件实现，可从单体架构到面向服务架构按需扩展

### 设计规范

1. 尽量使用.NET Standard和官方提供的类库，第三方类库设计成组件利用DI来按需组合。

### 安装Nuget包

``` doc
PM> Install-Package LinFx
````

### Samples

``` cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddLinFx()
        .AddDistributedRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("ReidsConnection");
        })
        .AddMongoDBContext(options =>
        {
            options.Name = "default";
            options.Configuration = configuration.GetConnectionString("MongoConnection");
        })
        .AddElasticsearch(options =>
        {
            options.DefaultIndex = "default";
            options.Host = "http://10.0.1.112:9200";
        });
}
``````
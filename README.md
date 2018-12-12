# LinFx 
[![](https://img.shields.io/badge/.NET%20Core-2.0.0-brightgreen.svg?style=flat-square)](https://www.microsoft.com/net/download/core) 
[![Build status](https://ci.appveyor.com/api/projects/status/33srpo7owl1h3y4e?svg=true)](https://ci.appveyor.com/project/rabbitmq/rabbitmq-dotnet-client)
[![GitHub license](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](https://github.com/linfx/LinFx/blob/master/LICENSE)

一个基于 .NET Core 2.0 开发的简单易用的快速开发框架，遵循领域驱动设计（DDD）规范约束，提供实现事件驱动、事件回溯、响应式等特性的基础设施。让开发者享受到正真意义的面向对象设计模式来带的美感。

## LinFx.Extensions

Caching、DapperExtensions、Elasticsearch、EventBus、Metrics、Mongo、RabbitMQ

### 特性

1. 领域驱动设计（DDD）
2. 事件驱动架构 (EDA)
3. 事件回溯 （ES）
4. 最终一致性 （Eventually Consistent）
6. 框架中每个组件都有基础实现，最简单时只需一个核心类库就能跑起来
7. 遵循端口与适配器模式，框架组件适配多种第三方组件实现，可从单体架构到面向服务架构按需扩展

### 知识点

1. 领域驱动设计（Domain Driven Design (Layers and Domain Model Pattern)
2. 命令查询职责分离（CQRS：Command Query Responsibility Segregation）
3. 领域通知 （Domain Notification）
5. 领域驱动 （Domain Events）
6. 事件驱动架构 (EDA)
7. 事件回溯 （Event Sourcing）
8. 最终一致性 （Eventually Consistent）
9. 工作单元模式 （Unit of Work ）
10. 泛型仓储 （Repository and Generic Repository）

### 设计规范

1. 尽量使用.NET Standard和官方提供的类库，第三方类库设计成组件利用DI来按需组合。

### 安装Nuget包

``` doc
PM> Install-Package LinFx
````

### 开发环境

1. Visual Studio 15.3+
2. .NET Core SDK 2.2+

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

### EventBus

``` cs

using LinFx.Extensions.EventBus.Abstractions;
using LinFx.Test.Extensions.EventBus.Events;
using LinFx.Utils;
using LinFx.Extensions.EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using LinFx.Test.Extensions.EventBus.EventHandling;
using System.Collections.Generic;
using System;

namespace LinFx.Test.Extensions.EventBus
{
    public class EventBusRabbitMQTest
    {
        private readonly IEventBus _eventBus;

        public EventBusRabbitMQTest()
        {
            var services = new ServiceCollection();

            services.AddLinFx()
                .AddEventBus(options =>
                {
                    options.Durable = true;
                    options.BrokerName = "tc_cloud_event_bus";
                    options.QueueName = "tc_cloud_process_queue";
                    options.ConfigureEventBus = (fx, builder) => builder.UseRabbitMQ(fx, x =>
                    {
                        x.Host = "14.21.34.85";
                        x.UserName = "admin";
                        x.Password = "admin.123456";
                    });
                });

            //services
            services.AddTransient<OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //services.AddTransient<OrderStatusChangedToPaidIntegrationEventHandler>();

            var applicationServices = services.BuildServiceProvider();

            //ConfigureEventBus
            _eventBus = applicationServices.GetRequiredService<IEventBus>();
            _eventBus.Subscribe<OrderStatusChangedToAwaitingValidationIntegrationEvent, OrderStatusChangedToAwaitingValidationIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStatusChangedToPaidIntegrationEvent, OrderStatusChangedToPaidIntegrationEventHandler>();
        }


        [Fact]
        public async Task Should_Call_Handler_On_Event_With_Correct_SourceAsync()
        {
            var orderId = Guid.NewGuid().GetHashCode() & ushort.MaxValue;
            var evt = new OrderStatusChangedToAwaitingValidationIntegrationEvent(orderId, new List<OrderStockItem>
            {
            });
            await _eventBus.PublishAsync(evt);

            //for (int i = 0; i < 2; i++)
            //{
            //    await _eventBus.PublishAsync(new ClientCreateIntergrationEvent
            //    {
            //        ClientId = IDUtils.CreateNewId().ToString(),
            //        ClientSecrets = new[] { "191d437f0cc3463b85669f2b570cdc21" },
            //        AllowedGrantTypes = new[] { "client_credentials" },
            //        AllowedScopes = new[] { "api3.device" }
            //    });
            //}
        }
    }
}

```
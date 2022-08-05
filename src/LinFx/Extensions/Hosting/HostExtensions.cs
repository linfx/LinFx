﻿//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Polly;

//namespace LinFx.Extensions.Hosting
//{
//    public static class HostExtensions
//    {
//        public static IHost MigrateDbContext<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder = default) where TContext : DbContext
//        {
//            using (var scope = host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;
//                var logger = services.GetRequiredService<ILogger<TContext>>();
//                var context = services.GetService<TContext>();

//                try
//                {
//                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

//                    var retry = Policy.Handle<Exception>()
//                         .WaitAndRetry(new TimeSpan[]
//                         {
//                             TimeSpan.FromSeconds(3),
//                             TimeSpan.FromSeconds(5),
//                             TimeSpan.FromSeconds(8),
//                         });

//                    retry.Execute(() =>
//                    {
//                        //if the sql server container is not created on run docker compose this
//                        //migration can't fail for network related exception. The retry options for DbContext only 
//                        //apply to transient exceptions.
//                        context.Database.Migrate();
//                        seeder?.Invoke(context, services);
//                    });

//                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
//                }
//                catch (Exception ex)
//                {
//                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
//                }
//            }
//            return host;
//        }
//    }
//}

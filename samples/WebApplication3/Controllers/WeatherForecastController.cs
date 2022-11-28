using EfPostgresql.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using Npgsql;
using System.Collections;
using System.Threading.Channels;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public void GetAsync() { }


        [HttpGet("pg")]
        public async Task Get1()
        {
            //await ch.Writer.WriteAsync(new testdata
            //{
            //    name = "a2222222222222",
            //    name1 = "b",
            //    name2 = "c",
            //});

            using var conn = new NpgsqlConnection("server=10.0.1.222;port=15433;database=db1;username=postgres;password=123456;Maximum Pool Size=200");
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO sensor_stat2 VALUES (1, 'f', '2022-11-17 16:18:38')";
            await cmd.ExecuteNonQueryAsync();
        }

        [HttpGet("mo")]
        public async Task Get2Async()
        {
            var database = client.GetDatabase("db1");
            var dt = database.GetCollection<testdata>("testdata");
            await dt.InsertOneAsync(new testdata
            {
                name = "a2222222222222",
                name1 = "b",
                name2 = "c",
            });

            //await ch.Writer.WriteAsync(new testdata
            //{
            //    name = "a2222222222222",
            //    name1 = "b",
            //    name2 = "c",
            //});
        }

        static MongoClient client;
        static MongoClientSettings config = MongoClientSettings.FromConnectionString("mongodb://10.0.1.222");

        static WeatherForecastController()
        {
            config.MaxConnecting = 10;
            config.MinConnectionPoolSize = 100;
            config.MaxConnectionPoolSize = 300;
            client = new MongoClient(config);

           //Parallel.ForEachAsync()

            for (int i = 0; i < 2; i++)
            {
                Task.Factory.StartNew(async () =>
                {
                    var database = client.GetDatabase("db1");
                    var dt = database.GetCollection<testdata>("testdata");

                    while (true)
                    {
                        var chuck = new List<testdata>();
                        var d = ch.Reader.ReadAllAsync();
                        await foreach (var item in d)
                        {
                            chuck.Add(item);
                            if (chuck.Count == 5)
                                break;
                        }

                        //await dt.InsertManyAsync(chuck);

                        using var conn = new NpgsqlConnection("server=10.0.1.222;port=15433;database=db1;username=postgres;password=123456;Maximum Pool Size=200");
                        await conn.OpenAsync();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO sensor_stat2 VALUES (1, 'f', '2022-11-17 16:18:38')";
                        //await cmd.ExecuteNonQueryAsync();
                    }
                });
            }

            Queue q = new Queue();
        }

        static Channel<testdata> ch = Channel.CreateUnbounded<testdata>();


    }
}
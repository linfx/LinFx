using EfPostgresql.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using MongoDB;
using MongoDB.Driver;

namespace EfPostgresql;

class Services : BackgroundService
{
    private readonly MyDbContext db;


    public Services(MyDbContext db)
    {
        this.db = db;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //var p = db.PointTests.ToList();


        //var pp = new PointTest
        //{
        //    Name = "pp1",
        //    Pt = new NetTopologySuite.Geometries.Point(1, 2, 3)
        //};


        //pp.Pt.X = 1;
        //pp.Pt.Y = 2;
        //pp.Pt.Z = 3;


        //db.PointTests.Add(pp);

        //try
        //{
        //    await db.SaveChangesAsync();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.ToString());
        //}

        return;

        var client = new MongoClient("mongodb://10.0.1.222");
        var database = client.GetDatabase("db1");
        var dt = database.GetCollection<testdata>("testdata");

        Stopwatch stopwatch = Stopwatch.StartNew();

        var count = 100;

        //for (int i = 0; i < count; i++)
        //{
        //    await dt.InsertOneAsync(new testdata
        //    {
        //        name = "a2222222222222",
        //        name1 = "b",
        //        name2 = "c",
        //    });
        //}


        //var rnd = new Random();


        //for (int i = 0; i < count; i++)
        //{
        //    db.Sensors.Add(new SensorStat
        //    {
        //        Sid = rnd.Next(1, 10000),
        //        State = false,
        //        CtrTime = DateTime.Now,
        //    });
        //    await db.SaveChangesAsync();
        //}
        //await db.SaveChangesAsync();


        for (int i = 0; i < count; i++)
        {
            db.Database.ExecuteSqlRaw("INSERT INTO sensor_stat2 VALUES (1, 'f', '2022-11-17 16:18:38')");
        }


        stopwatch.Stop();

        Console.WriteLine($"数据量 {count}: ,插入时间: {stopwatch.ElapsedMilliseconds} ms");

        //Console.WriteLine(p[0].Name);

        
     }
}

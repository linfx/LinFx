using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPostgresql.Domain;

[Table("sensor_stat1")]
public class SensorStat
{
    [Column("sid")]
    public int Sid { get; set; }

    [Column("state")]
    public bool State { get; set; }

    [Column("crt_time")]
    public DateTime CtrTime { get; set; }
}

[Table("point_test")]
public class PointTest
{
    [Key, Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("pt")]
    public Point Pt { get; set; }
}

public class testdata
{
    public int x { get; set; }
    public string name { get; set; }
    public string name1 { get; set; }
    public string name2 { get; set; }
}
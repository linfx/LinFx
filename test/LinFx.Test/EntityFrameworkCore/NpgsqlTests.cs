using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using LinFx.Extensions.EntityFrameworkCore;

namespace LinFx.Test.EntityFrameworkCore
{
    public class NpgsqlTests
    {
        ApplicationDbContext _db;

        public NpgsqlTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>();
        }

        [Fact]
        public async Task ModityAsync()
        {
            //_db.Modity()
        }

    }
}

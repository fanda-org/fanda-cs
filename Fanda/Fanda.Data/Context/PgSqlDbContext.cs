using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanda.Data.Context
{
    public class PgSqlDbContext : FandaContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseNpgsql("Data Source=my.db");
        public PgSqlDbContext(DbContextOptions<PgSqlDbContext> options) : base(options)
        {
        }
    }
}

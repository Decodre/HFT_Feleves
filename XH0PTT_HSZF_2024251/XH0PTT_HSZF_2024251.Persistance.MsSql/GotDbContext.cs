using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XH0PTT_HSZF_2024251.Model;

namespace XH0PTT_HSZF_2024251.Persistance.MsSql
{
    public class GotDbContext : DbContext
    {
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public GotDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=gotdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(conn);

            base.OnConfiguring(optionsBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ParkingLotCommonLayer.ModelDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.ApplicationContext
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { set; get; }
        public DbSet<ParkingDetails> ParkingDetails { set; get; }
        public DbSet<UnParkedDetails> UnParkedDetails { set; get; }
    }
}

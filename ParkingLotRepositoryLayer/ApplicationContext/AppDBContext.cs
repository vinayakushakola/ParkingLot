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
    }
}

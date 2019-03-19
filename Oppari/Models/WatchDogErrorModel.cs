using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oppari.Models
{
    public class WatchDogErrorContext : DbContext
    {
        public WatchDogErrorContext()
        {

        }

        public WatchDogErrorContext(DbContextOptions<WatchDogErrorContext> options) : base(options)
        {

        }

        public DbSet<WatchDogErrorModel> WatchDogErrors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=WatchDog_Db;Trusted_Connection=True;ConnectRetryCount=0");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WatchDogErrorModel>(entity =>
            {
                // Set key for entity
                entity.HasKey(e => e.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
    

    public class WatchDogErrorModel
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public int Status { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }

        public WatchDogErrorModel()
        {

        }

        public WatchDogErrorModel(string methodName, string errorMessage, int status, DateTime timestamp, string parameter1 = "", string parameter2 = "", string parameter3 = "", string parameter4 = "", string parameter5 = "")
        {
            MethodName = methodName;
            ErrorMessage = errorMessage;
            Status = status;
            TimeStamp = timestamp;
            Parameter1 = parameter1;
            Parameter2 = parameter2;
            Parameter3 = parameter3;
            Parameter4 = parameter4;
            Parameter5 = parameter5;
        }

    }
}

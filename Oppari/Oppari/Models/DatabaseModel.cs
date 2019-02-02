using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Oppari.Models
{
    public class ComputerBuildingContext : DbContext
    {
        public ComputerBuildingContext()
        {

        }

        public ComputerBuildingContext(DbContextOptions<ComputerBuildingContext> options) : base(options)
        {

        }

        public DbSet<Build> Builds { get; set; }
        public DbSet<Component> Components { get; set; }
    }

    public class Build
    {
        public int BuildId { get; set; }
        public string Title { get; set; }

        public ICollection<Component> Components { get; set; }
    }

    public class Component
    {
        public int ComponentId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public int BuildId { get; set; }
        public Build Build { get; set; }
    }
}

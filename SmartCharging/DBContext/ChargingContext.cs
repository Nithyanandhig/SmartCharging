using Microsoft.EntityFrameworkCore;
using SmartCharging.Model;
using System.Reflection.Metadata;

namespace SmartCharging.DBContext
{
    public class ChargingContext : DbContext
    {
        public ChargingContext(DbContextOptions<ChargingContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Connector>()
        .HasKey(p => new { p.ConnectorId, p.StationId });
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ChargingStation> Stations { get; set; }
        public DbSet<Connector> Connectors { get; set; }
    }
}

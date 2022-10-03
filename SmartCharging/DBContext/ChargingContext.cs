using Microsoft.EntityFrameworkCore;
using SmartCharging.Model;

namespace SmartCharging.DBContext
{
    public class ChargingContext : DbContext
    {
        public ChargingContext(DbContextOptions<ChargingContext> options): base(options)
        {

        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ChargingStation> Stations { get; set; }
        public DbSet<Connector> Connectors { get; set; }
    }
}

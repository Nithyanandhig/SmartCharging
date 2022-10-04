using SmartCharging.DBContext;
using SmartCharging.Model;

namespace SmartCharging
{
    public class DataSeeder
    {
        private readonly ChargingContext _context;

        public DataSeeder(ChargingContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            var connectors = new List<Connector>();
            connectors.Add(new Model.Connector { ConnectorId = 1, StationId = 1, MaxCurrentInAmps = 10 });
            connectors.Add(new Model.Connector { ConnectorId = 2, StationId = 1, MaxCurrentInAmps = 15 });
            connectors.Add(new Model.Connector { ConnectorId = 3, StationId = 1, MaxCurrentInAmps = 20 });

            connectors.Add(new Model.Connector { ConnectorId = 1, StationId = 2, MaxCurrentInAmps = 10 });
            connectors.Add(new Model.Connector { ConnectorId = 2, StationId = 2, MaxCurrentInAmps = 15 });
            connectors.Add(new Model.Connector { ConnectorId = 3, StationId = 2, MaxCurrentInAmps = 20 });

            connectors.Add(new Model.Connector { ConnectorId = 1, StationId = 3, MaxCurrentInAmps = 15 });
            connectors.Add(new Model.Connector { ConnectorId = 2, StationId = 3, MaxCurrentInAmps = 30 });
            connectors.Add(new Model.Connector { ConnectorId = 3, StationId = 3, MaxCurrentInAmps = 25 });

            connectors.Add(new Model.Connector { ConnectorId = 1, StationId = 4, MaxCurrentInAmps = 12 });
            connectors.Add(new Model.Connector { ConnectorId = 2, StationId = 4, MaxCurrentInAmps = 16 });
            connectors.Add(new Model.Connector { ConnectorId = 3, StationId = 4, MaxCurrentInAmps = 21 });

            var stations = new List<ChargingStation>();
            stations.Add(new Model.ChargingStation { Id = 1, GroupId = 1, Name = "CS1", Connectors = connectors.Where(a => a.StationId == 1).ToList() });
            stations.Add(new Model.ChargingStation { Id = 2, GroupId = 1, Name = "CS2", Connectors= connectors.Where(a => a.StationId == 2).ToList() });

            stations.Add(new Model.ChargingStation { Id = 3, GroupId = 2, Name = "CS3", Connectors = connectors.Where(a => a.StationId == 3).ToList() });
            stations.Add(new Model.ChargingStation { Id = 4, GroupId = 2, Name = "CS4", Connectors = connectors.Where(a => a.StationId == 4).ToList() });

            var groups = new List<Group>();
            groups.Add(new Group { Id = 1, Name = "Hyd", CapacityInAmps = 1000, Stations = stations.Where(a => a.GroupId == 1).ToList() });
            groups.Add(new Group { Id = 2, Name = "Chn", CapacityInAmps = 1200, Stations = stations.Where(a => a.GroupId == 2).ToList() });

            _context.Connectors.AddRange(connectors);
            _context.Stations.AddRange(stations);
            _context.Groups.AddRange(groups);

            _context.SaveChanges();
        }
    }
}

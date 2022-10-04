using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Model;
using SmartCharging.Services;

namespace SmartCharging.Tests.Repositories
{
    public class StationRepositoryTests
    {
        private readonly ChargingContext _virtualContext;
        private IStationService stationService;
        private ICommonService commonService;
        public StationRepositoryTests()
        {
            var dbName = "Charging_" + DateTime.Now.ToFileTimeUtc();
            _virtualContext = new ChargingContext(
                new DbContextOptionsBuilder<ChargingContext>()
                   .UseInMemoryDatabase(databaseName: dbName)
                   .Options
                );
            DataSeeder seeder = new DataSeeder(_virtualContext);
            seeder.Seed();
            stationService = new StationService(_virtualContext);
        }

        [Fact(DisplayName = "Fetch all the station in the Group")]
        public void FetchStationsByGroupId_return_valid_model()
        {
            List<ChargingStation> stations = stationService.FetchStationsByGroupId(1).Result;
            Assert.Equal(2, stations.Count());
        }

        [Fact(DisplayName = "Station Already Exists")]
        public void AddStation_Station_Not_exists()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => stationService.AddStation(new ChargingStation { Id=3,GroupId=1,Name="CS3"}));
            Assert.Equal("Station Already Exists", exception.Result.Message);
        }

        [Fact(DisplayName = "Group Not Exists to Add Station")]
        public void AddStation_Group_Not_exists()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => stationService.AddStation(new ChargingStation { Id = 3, GroupId = 3, Name = "CS3" }));
            Assert.Equal("Group does not Exists", exception.Result.Message);
        }

        [Fact(DisplayName = "Connectors should be between 1-5")]
        public void AddStation_Connector_Validate()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => stationService.AddStation(new ChargingStation { Id = 5, GroupId = 1, Name = "CS5", Connectors = new List<Connector>() }));
            Assert.Equal("Station Should have atleast one Connector and not more than 5", exception.Result.Message);
        }

        [Fact(DisplayName = "Station Save Success")]
        public void AddStation_Connector_Valid()
        {
            List<Connector> connectors = new List<Connector>();
            connectors.Add(new Connector { ConnectorId = 1, StationId = 5, MaxCurrentInAmps = 2 });

            ChargingStation station = stationService.AddStation(new ChargingStation { Id = 5, GroupId = 1, Name = "CS5", Connectors = connectors  }).Result;
            Assert.Equal(5, _virtualContext.Stations.Count());
        }

        [Fact(DisplayName = "Group Not Exists to Update Station")]
        public void UpdateStation_Group_Not_exists()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => stationService.UpdateStation(4,new ChargingStation { Id = 4, GroupId = 4, Name = "CS3" }));
            Assert.Equal("Group does not Exists", exception.Result.Message);
        }

        [Fact(DisplayName = "Update Station Success")]
        public void UpdateStation_Success()
        {
            ChargingStation station = stationService.UpdateStation(3, new ChargingStation { Id = 3, GroupId = 1, Name = "CSS3" }).Result;
            Assert.Equal("CSS3", station.Name);
        }


        [Fact(DisplayName = "Delete Station Success")]
        public void DeleteStation_Success()
        {
            ChargingStation station = stationService.DeleteStation(3).Result;
            Assert.NotNull(station);
        }
    }
}

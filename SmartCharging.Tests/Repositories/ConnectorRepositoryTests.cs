using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Model;
using SmartCharging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCharging.Tests.Repositories
{
    public class ConnectorRepositoryTests
    {
        private readonly ChargingContext _virtualContext;
        private IConnectorService connectorService;
        private ICommonService commonService;
        public ConnectorRepositoryTests()
        {
            var dbName = "Charging_" + DateTime.Now.ToFileTimeUtc();
            _virtualContext = new ChargingContext(
                new DbContextOptionsBuilder<ChargingContext>()
                   .UseInMemoryDatabase(databaseName: dbName)
                   .Options
                );
            DataSeeder seeder = new DataSeeder(_virtualContext);
            seeder.Seed();
            commonService = new CommonService(_virtualContext);
            connectorService = new ConnectorService(_virtualContext, commonService);
        }

        [Fact(DisplayName = "Fetch All Connectors In Station")]
        public void FetchAllConnectorsInStation_return_valid()
        {
            List<Connector> connectors = connectorService.FetchAllConnectorsInStation(2).Result;
            Assert.Equal(3, connectors.Count());
        }

        [Fact(DisplayName = "Delete Station Success")]
        public void DeleteConnector_Success()
        {
            Connector connector = connectorService.DeleteConnector(3, 2).Result;
            Assert.NotNull(connector);
        }

        [Fact(DisplayName = "Not a Valid Station")]
        public void UpdateConnector_Station_Not_exists()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => connectorService.UpdateConnector(4, new Connector { StationId = 9, ConnectorId = 1, MaxCurrentInAmps = 10 }));
            Assert.Equal("Station does not exists", exception.Result.Message);
        }

        [Fact(DisplayName = "UpdateConnector Capacity Validate")]
        public void UpdateConnector_Capacity_Validate()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => connectorService.UpdateConnector(4, new Connector { StationId = 3, ConnectorId = 1, MaxCurrentInAmps = 10000 }));
            Assert.Equal("MaxCurrent exceeds the total capacity", exception.Result.Message);
        }

        [Fact(DisplayName = "UpdateConnector Success")]
        public void UpdateConnector_Success()
        {
            Connector connector = connectorService.UpdateConnector(1, new Connector { StationId = 1, ConnectorId = 1, MaxCurrentInAmps = 10 }).Result;
            Assert.NotNull(connector);
        }

        [Fact(DisplayName = "AddConnector Success")]
        public void AddConnector_Success()
        {
            Connector connector = connectorService.AddConnector( new Connector { StationId = 4, ConnectorId = 4, MaxCurrentInAmps = 10 }).Result;
            Assert.NotNull(connector);
        }

        [Fact(DisplayName = "Not a Valid Station")]
        public void AddConnector_Station_Not_exists()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => connectorService.AddConnector(new Connector { StationId = 9, ConnectorId = 1, MaxCurrentInAmps = 10 }));
            Assert.Equal("Station does not exists", exception.Result.Message);
        }

        [Fact(DisplayName = "AddConnector Capacity Validate")]
        public void AddConnector_Capacity_Validate()
        {
            var exception = Assert.ThrowsAsync<ApplicationException>(() => connectorService.AddConnector(new Connector { StationId = 3, ConnectorId = 1, MaxCurrentInAmps = 10000 }));
            Assert.Equal("MaxCurrent exceeds the total capacity", exception.Result.Message);
        }
    }
    }

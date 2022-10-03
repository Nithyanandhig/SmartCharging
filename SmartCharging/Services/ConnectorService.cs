using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Model;

namespace SmartCharging.Services
{
    public class ConnectorService : IConnectorService
    {
        private ChargingContext _context;
        private ICommonService _commonService;

        public ConnectorService(ChargingContext context, ICommonService commonService)
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<List<Connector>> FetchAllConnectorsInStation(int stationId)
        {
            var connectors = await _context.Connectors
                                  .Where(c => c.StationId == stationId)
                                  .ToListAsync(); ;
            return connectors;
        }

        public async Task<Connector> AddConnector(Connector connector)
        {
            var station = _context.Stations.FindAsync(connector.StationId).Result;
            if (station is null)
            {
                throw new ApplicationException("Station Does not exists");
            }
            else
            {
                if (_commonService.IsExceedMaxCapacity(station.GroupId))
                {
                    _context.Connectors.Add(connector);
                    await _context.SaveChangesAsync();
                }
            }
            return connector;
        }

        public async Task<Connector> UpdateConnector(int id, Connector connector)
        {
            var modifiedConnector = await _context.Connectors
             .Where(x => x.Id == id && x.StationId == connector.StationId).FirstOrDefaultAsync();
            var station = _context.Stations.FindAsync(connector.StationId).Result;
            if (station is null)
            {
                throw new ApplicationException("Station Does not exists");
            }
            if (modifiedConnector is not null && _commonService.IsExceedMaxCapacity(station.GroupId))
            {
                modifiedConnector.MaxCurrentInAmps = connector.MaxCurrentInAmps;
                await _context.SaveChangesAsync();
            }
            return modifiedConnector;
        }

        public async Task<Connector> DeleteConnector(int connectorId,int stationId)
        {
            var connector = await _context.Connectors.FirstOrDefaultAsync(a=> a.Id == connectorId && a.StationId == stationId);
            if (connector is not null)
            {
                _context.Connectors.Remove(connector);
                await _context.SaveChangesAsync();
            }
            return connector;
        }

    
    }
}

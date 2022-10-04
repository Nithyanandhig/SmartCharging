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
            return await _context.Connectors
                                  .Where(c => c.StationId == stationId)
                                  .ToListAsync(); ;
        }

        public async Task<Connector> AddConnector(Connector connector)
        {
            var station = _context.Stations.FindAsync(connector.StationId).Result;
            if (station is null)
            {
                throw new ApplicationException("Station does not exists");
            }
            else
            {
                if (_context.Connectors.Where(a => a.StationId == connector.StationId).Count() < 5)
                {
                    if (_commonService.IsTotalCurrentLessThanCapacity(station.GroupId, connector.MaxCurrentInAmps,station.Id))
                    {
                        _context.Connectors.Add(connector);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        throw new ApplicationException("MaxCurrent exceeds the total capacity");
                    }
                }
                else
                {
                    throw new ApplicationException("Cannot add counter more than 5");
                }
            }
            return connector;
        }

        public async Task<Connector> UpdateConnector(int id, Connector connector)
        {
            var modifiedConnector = await _context.Connectors
             .Where(x => x.ConnectorId == id && x.StationId == connector.StationId).FirstOrDefaultAsync();
            var station = _context.Stations.FindAsync(connector.StationId).Result;
            if (station is null)
            {
                throw new ApplicationException("Station does not exists");
            }
            if (modifiedConnector is not null && _commonService.IsTotalCurrentLessThanCapacity(station.GroupId,connector.MaxCurrentInAmps,station.Id,id))
            {
                modifiedConnector.MaxCurrentInAmps = connector.MaxCurrentInAmps;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ApplicationException("MaxCurrent exceeds the total capacity");
            }
            return modifiedConnector;
        }

        public async Task<Connector> DeleteConnector(int connectorId,int stationId)
        {
            var connector = await _context.Connectors.FirstOrDefaultAsync(a=> a.ConnectorId == connectorId && a.StationId == stationId);
            if (connector is not null)
            {
                _context.Connectors.Remove(connector);
                await _context.SaveChangesAsync();
            }
            return connector;
        }

    
    }
}

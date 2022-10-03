using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Model;
using System.Text.RegularExpressions;

namespace SmartCharging.Services
{
    public class StationService : IStationService
    {
        private ChargingContext _context;
        public StationService(ChargingContext context)
        {
            _context = context;
        }

        public async Task<List<ChargingStation>> FetchStationsByGroupId(int groupId)
        {
            var stations = await _context.Stations
                               .Where(c => c.GroupId == groupId)
                               .ToListAsync(); 
            return stations;
        }
        public async Task<ChargingStation> FetchStationById(int stationId)
        {
            var stations = await _context.Stations.FindAsync(stationId);
            return stations;
        }
        public async Task<ChargingStation> AddStation(ChargingStation station)
        {
            if(hasStationAlready(station))
            {
                throw new ApplicationException("Station Already Exists");
            }
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return station;
        }
        public async Task<ChargingStation> UpdateStation(int id, ChargingStation station)
        {
            if (hasStationAlready(station))
            {
                throw new ApplicationException("Station Already Exists");
            }
            var updateStation = await _context.Stations
           .FindAsync(id);
            if (updateStation is not null)
            {
                updateStation.Name = station.Name;
                await _context.SaveChangesAsync();
            }
            return updateStation;
        }
        public async Task<ChargingStation> DeleteStation(int stationId)
        {
            var station = await _context.Stations.FindAsync(stationId);
            if (station is not null)
            {
                var connectors = _context.Connectors.Where(a => a.StationId == stationId).ToArray();
                _context.Connectors.RemoveRange(connectors);
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
            return station;
        }

        private bool hasStationAlready(ChargingStation station)
        {
            return _context.Stations.Any(a => a.GroupId == station.GroupId && a.Name == station.Name);
        }
    }
}

using SmartCharging.Model;

namespace SmartCharging.Interfaces
{
    public interface IStationService
    {
        Task<List<ChargingStation>> FetchStationsByGroupId(int groupId);
        Task<ChargingStation> FetchStationById(int stationId);
        Task<ChargingStation> AddStation(ChargingStation station);
        Task<ChargingStation> UpdateStation(int id, ChargingStation station);
        Task<ChargingStation> DeleteStation(int stationId);
    }
}

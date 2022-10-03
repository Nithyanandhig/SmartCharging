using SmartCharging.Model;

namespace SmartCharging.Interfaces
{
    public interface IConnectorService
    {
        Task<List<Connector>> FetchAllConnectorsInStation(int stationId);
        Task<Connector> AddConnector(Connector connector);
        Task<Connector> UpdateConnector(int id, Connector connector);
        Task<Connector> DeleteConnector(int connectorId, int stationId);

    }
}

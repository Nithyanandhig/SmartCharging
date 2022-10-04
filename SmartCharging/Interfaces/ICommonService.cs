namespace SmartCharging.Interfaces
{
    public interface ICommonService
    {
        bool IsMaxCapacityHigh(int groupId, double maxCapacity);

        bool IsTotalCurrentLessThanCapacity(int groupId, double newCurrent, int connectorId = 0);
    }
}

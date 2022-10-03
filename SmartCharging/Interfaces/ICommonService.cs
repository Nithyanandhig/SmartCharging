namespace SmartCharging.Interfaces
{
    public interface ICommonService
    {
        bool IsExceedMaxCapacity(int groupId, double maxCapcity=0);
    }
}

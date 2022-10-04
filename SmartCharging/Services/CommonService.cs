using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;

namespace SmartCharging.Services
{
    public class CommonService : ICommonService
    {
        private ChargingContext _context;

        public CommonService(ChargingContext context)
        {
            _context = context;
        }

        public bool IsMaxCapacityHigh(int groupId, double maxCapacity)
        {
            if (maxCapacity >= GetExistingCurrentInAmps(groupId))
                return true;
            return false;
        }

        public bool IsTotalCurrentLessThanCapacity(int groupId,double newCurrent,int stationId,int connectorId= 0)
        {
            var totalCapcity = _context.Groups.Where(a => a.Id == groupId).SingleOrDefault().CapacityInAmps;
            double existingTotalCurrent = GetExistingCurrentInAmps(groupId);
            double totalCurrent = connectorId > 0 ? (existingTotalCurrent -  _context.Connectors.Find(connectorId,stationId).MaxCurrentInAmps + newCurrent) : (existingTotalCurrent + newCurrent);
            if (totalCurrent <= totalCapcity)
                return true;
            return false;
        }

        private double GetExistingCurrentInAmps(int groupId)
        {
            var group = _context.Groups.Where(a => a.Id == groupId).Include(b => b.Stations).ThenInclude(c => c.Connectors).FirstOrDefault();
            double totalCurrent = 0;
            foreach (var station in group.Stations)
            {
                double total = station.Connectors.Sum(b => b.MaxCurrentInAmps);
                totalCurrent = totalCurrent + total;
            }
            return totalCurrent;
        }
    }
}

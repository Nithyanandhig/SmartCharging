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

        public bool IsExceedMaxCapacity(int groupId, double capacity=0)
        {
            var stations = _context.Stations.Where(a => a.GroupId == groupId).ToArray();
            double totalCurrent=0;
            foreach (var station in stations)
            {
                double total = _context.Connectors.Where(a => a.StationId == station.Id).Sum(b => b.MaxCurrentInAmps);
                totalCurrent = totalCurrent + total;
            }
            if(capacity <=0)
            {
                capacity = _context.Groups.Where(a => a.Id == groupId).SingleOrDefault().CapacityInAmps;
            }
            if (capacity >= totalCurrent)
                return true;
            return false;

        }
    }
}

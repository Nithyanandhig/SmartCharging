using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using Group = SmartCharging.Model.Group;

namespace SmartCharging.Services
{
    public class GroupService : IGroupService
    {
        private ChargingContext _context;
        private ICommonService _commonService;

        public GroupService(ChargingContext context, ICommonService commonService)  
        {
            _context = context;
            _commonService = commonService;
        }

        public async Task<Group> FetchGroupById(int groupId)
        {
            var product = await _context.Groups
                                  .Where(x => x.Id == groupId)
                                  .SingleOrDefaultAsync(); 
            return product;
        }

        public async Task<Group> AddGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> UpdateGroup(int id, Group group)
        {
            var updateGroup = await _context.Groups
             .FindAsync(id);
            if (updateGroup is not null)
            {
                if (_commonService.IsExceedMaxCapacity(id, group.CapacityInAmps))
                {
                    updateGroup.CapacityInAmps = group.CapacityInAmps;
                    updateGroup.Name = group.Name;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ApplicationException("Group Capacity is lesser than total current in the connectors");
                }
            }
            return updateGroup;
        }

        public async Task<Group> DeleteGroup(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group is not null)
            {
                var stations = _context.Stations.Where(g => g.GroupId == groupId).ToArray();
                foreach (var station in stations)
                {
                    var connectors = _context.Connectors.Where(g => g.StationId == station.Id).ToArray();
                    _context.Connectors.RemoveRange(connectors);
                }
                _context.Stations.RemoveRange(stations);
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            return group;
        }
    }
}

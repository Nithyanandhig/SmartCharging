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
            return await _context.Groups
                                  .Where(x => x.Id == groupId)
                                  .Include(x=>x.Stations)
                                  .ThenInclude(c=>c.Connectors)
                                  .SingleOrDefaultAsync(); 
            
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
                if (_commonService.IsMaxCapacityHigh(id, group.CapacityInAmps))
                {
                    updateGroup.CapacityInAmps = group.CapacityInAmps;
                    updateGroup.Name = group.Name;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ApplicationException("total capacity should not be lesser than the total current avaialble in the connectors");
                }
            }
            return updateGroup;
        }

        public async Task<Group> DeleteGroup(int groupId)
        {
            var group = await _context.Groups.Where(a=>a.Id==groupId).Include(x => x.Stations)
                                  .ThenInclude(c => c.Connectors).FirstOrDefaultAsync();
            if (group is not null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
            return group;
        }
    }
}

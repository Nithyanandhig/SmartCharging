using SmartCharging.Model;

namespace SmartCharging.Interfaces
{
    public interface IGroupService
    {
        Task<Group> FetchGroupById(int groupId);

        Task<Group> AddGroup(Group group);

        Task<Group> UpdateGroup(int id,Group group);

        Task<Group> DeleteGroup(int groupId);

    }
}

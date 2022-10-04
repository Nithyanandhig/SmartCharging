using Microsoft.EntityFrameworkCore;
using SmartCharging.DBContext;
using SmartCharging.Interfaces;
using SmartCharging.Model;
using SmartCharging.Services;

namespace SmartCharging.Tests.Controllers
{
    public class GroupRepositoryTests
    {

        private readonly ChargingContext _virtualContext;
        private IGroupService groupService;
        private ICommonService commonService;
        public GroupRepositoryTests()
        {
            var dbName = "Charging_" + DateTime.Now.ToFileTimeUtc();
            _virtualContext = new ChargingContext(
                new DbContextOptionsBuilder<ChargingContext>()
                   .UseInMemoryDatabase(databaseName: dbName)
                   .Options
                );
            DataSeeder seeder = new DataSeeder(_virtualContext);
            seeder.Seed();
            commonService = new CommonService(_virtualContext);
            groupService = new GroupService(_virtualContext, commonService);
        }

        [Fact(DisplayName = "FetchGroupById should return valid model")]
        public void FetchGroupById_return_valid_model()
        {
            Group group = groupService.FetchGroupById(1).Result;
            Assert.NotNull(group);
            Assert.Equal("Hyd", group.Name);
        }

        [Fact(DisplayName = "FetchGroupById should return null if there is no group")]
        public void FetchGroupById_return_null()
        {
            Group group = groupService.FetchGroupById(3).Result;
            Assert.Null(group);
        }

        [Fact(DisplayName = "AddGroup Success")]
        public void AddGroup_return_Success()
        {
            Group group = groupService.AddGroup(new Group { Id = 3, Name = "Blr", CapacityInAmps = 1000 }).Result;
            Assert.NotNull(group);
            Assert.Equal(3, _virtualContext.Groups.Count());
        }

        [Fact(DisplayName = "UpdateGroup Success")]
        public void UpdateGroup_return_Success()
        {
            Group group = groupService.UpdateGroup(2,new Group { Id = 2, Name = "chnn", CapacityInAmps = 1000 }).Result;
            Assert.Equal("chnn", group.Name);
        }


        [Fact(DisplayName = "Group does not exists")]
        public void UpdateGroup_does_not_exits()
        {
            Group group = groupService.UpdateGroup(3, new Group { Id = 3, Name = "Blrr", CapacityInAmps = 0 }).Result;
            Assert.Null(group);
        }

        [Fact(DisplayName = "Total capacity should be greater than total current")]
        public void UpdateGroup_return_total_capacity_warning()
        {
           var exception = Assert.ThrowsAsync<ApplicationException>(() => groupService.UpdateGroup(2, new Group { Id = 2, Name = "Blrr", CapacityInAmps = 0 }));
           Assert.Equal("total capacity should not be lesser than the total current avaialble in the connectors",exception.Result.Message);
        }

        [Fact(DisplayName = "DeleteGroup Success")]
        public void DeleteGroup_return_Success()
        {
            Group group = groupService.DeleteGroup(3).Result;
            Assert.Equal(2, _virtualContext.Groups.Count());
        }

        [Fact(DisplayName = "No Group to delete")]
        public void DeleteGroup_Failure()
        {
            Group group = groupService.DeleteGroup(4).Result;
            Assert.Equal(2, _virtualContext.Groups.Count());
        }
    }
}
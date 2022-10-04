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
            _virtualContext = new ChargingContext(
                new DbContextOptionsBuilder<ChargingContext>()
                   .UseInMemoryDatabase(databaseName: "Charging")
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
    }
}
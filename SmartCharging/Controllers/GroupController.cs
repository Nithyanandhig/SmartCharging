using Microsoft.AspNetCore.Mvc;
using SmartCharging.Interfaces;
using Group = SmartCharging.Model.Group;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupController : ControllerBase
    {

        private IGroupService _gropuService;
        private readonly ILogger<GroupController> _logger;

        public GroupController(ILogger<GroupController> logger,IGroupService groupService)
        {
            _logger = logger;
           _gropuService = groupService;    
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> FetchGroupById (int groupId)
        {    
            try
            {
                var group = await _gropuService.FetchGroupById(groupId);
                if (group is null)
                    return NotFound();
                return Ok(group);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(Group group)
        {
            try
            {
                if (group == null)
                    return BadRequest();

                var newGroup = await _gropuService.AddGroup(group);

                return Ok(newGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, Group group)
        {
            try
            {
                if (groupId <= 0 && groupId == group.Id)
                    return BadRequest();

                var status = await _gropuService.UpdateGroup(groupId, group);

                if (status is null)
                {
                    return NotFound();
                }
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                if (groupId <= 0)
                    return BadRequest();

                var status = await _gropuService.DeleteGroup(groupId);

                if (status is null)
                {
                    return NotFound();
                }
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred with a message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
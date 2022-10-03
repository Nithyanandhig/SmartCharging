using Microsoft.AspNetCore.Mvc;
using SmartCharging.Interfaces;
using SmartCharging.Model;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargingStationController : ControllerBase
    {
        private IStationService _stationService;

        public ChargingStationController(IStationService stationService)
        {
            _stationService = stationService;
        }

        [HttpGet("{groupId}/stations")]
        public async Task<IActionResult> FetchStationsByGroupId(int groupId)
        {
            try
            {
                var station = await _stationService.FetchStationsByGroupId(groupId);
                if (station is null)
                    return NotFound();
                return Ok(station);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{stationId}")]
        public async Task<IActionResult> FetchStationById(int stationId)
        {
            try
            {
                var station = await _stationService.FetchStationById(stationId);
                if (station is null)
                    return NotFound();
                return Ok(station);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddStation(ChargingStation station)
        {
            try
            {
                if (station == null)
                    return BadRequest();

                var newStation = await _stationService.AddStation(station);

                return Ok(newStation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{stationId}")]
        public async Task<IActionResult> UpdateStation(int stationId, ChargingStation station)
        {
            try
            {
                if (stationId <= 0 && stationId == station.Id)
                    return BadRequest();

                var status = await _stationService.UpdateStation(stationId, station);

                if (status is null)
                {
                    return NotFound();
                }
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{stationId}")]
        public async Task<IActionResult> DeleteStation(int stationId)
        {
            try
            {
                if (stationId <= 0)
                    return BadRequest();

                var status = await _stationService.DeleteStation(stationId);

                if (status is null)
                {
                    return NotFound();
                }
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

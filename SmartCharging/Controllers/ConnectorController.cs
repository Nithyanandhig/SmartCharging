using Microsoft.AspNetCore.Mvc;
using SmartCharging.Interfaces;
using SmartCharging.Model;
using SmartCharging.Services;

namespace SmartCharging.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectorController : ControllerBase
    {
        private IConnectorService _connectorService;

        public ConnectorController(IConnectorService connectorService)
        {
            _connectorService = connectorService;
        }


        [HttpGet("{stationId}")]
        public async Task<IActionResult> FetchAllConnectorsInStation(int stationId)
        {
            try
            {
                var station = await _connectorService.FetchAllConnectorsInStation(stationId);
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
        public async Task<IActionResult> AddConnector(Connector station)
        {
            try
            {
                if (station == null)
                    return BadRequest();

                var newStation = await _connectorService.AddConnector(station);

                return Ok(newStation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConnector(int id, Connector connector)
        {
            try
            {
                if (id <= 0 && id == connector.ConnectorId)
                    return BadRequest();

                var status = await _connectorService.UpdateConnector(id, connector);

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

        [HttpDelete("{id}/{stationId}")]
        public async Task<IActionResult> DeleteConnector(int id,int stationId)
        {
            try
            {
                if (id <= 0)
                    return BadRequest();

                var status = await _connectorService.DeleteConnector(id, stationId);

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

using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Models.Responses;
using MetricWebApi_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi_Manager.Controllers
{
    [Route("api/ram")]
    [ApiController]
    public class RAMMetricController : ControllerBase
    {
        private readonly IMetricsAgentClient _metricsAgentClient;

        public RAMMetricController(IMetricsAgentClient metricsAgentClient)
        {
            _metricsAgentClient = metricsAgentClient;
        }

        [HttpGet("getRAMMetricsFromAgent")]
        [ProducesResponseType(typeof(RAMMetricsResponse), StatusCodes.Status200OK)]
        public IActionResult GetMetricsFromRequest([FromBody] RAMMetricsRequest request)
        {
            RAMMetricsResponse response = _metricsAgentClient.GetRAMMetrics(new RAMMetricsRequest()
            {
                AgentId = request.AgentId,
                FromTime = request.FromTime,
                ToTime = request.ToTime
            });

            return Ok(response);
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromRoute([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            RAMMetricsResponse response = _metricsAgentClient.GetRAMMetrics(new RAMMetricsRequest()
            {
                AgentId = agentId,
                FromTime = fromTime,
                ToTime = toTime
            });

            return Ok(response);
        }
    }
}

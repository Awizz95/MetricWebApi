using MetricWebApi_Manager.Models;
using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi_Manager.Controllers
{
    public class CPUMetricController
    {
        [Route("api/cpu")]
        [ApiController]
        public class CpuMetricsController : ControllerBase
        {
            private readonly IMetricsAgentClient _metricsAgentClient;

            public CpuMetricsController(IMetricsAgentClient metricsAgentClient)
            {
                _metricsAgentClient = metricsAgentClient;
            }

            [HttpGet("getCpuMetricsFromAgent")]
            [ProducesResponseType(typeof(CpuMetricsResponse), StatusCodes.Status200OK)] //тип объекта, если ответ от сервиса успешный
            public IActionResult GetMetricsFromAgentV2([FromBody] CpuMetricsRequest request)
            {
                CpuMetricsResponse response = _metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest()
                {
                    AgentId = request.AgentId,
                    FromTime = request.FromTime,
                    ToTime = request.ToTime
                });
                return Ok(response);
            }

            [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
            public IActionResult GetMetricsFromAgentV1([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
            {
                CpuMetricsResponse response = _metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest()
                {
                    AgentId = agentId,
                    FromTime = fromTime,
                    ToTime = toTime
                });
                return Ok(response);
            }
        }
    }
}

using MetricWebApi_Manager.Models;
using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Responses;
using MetricWebApi_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi_Manager.Controllers
{
    [Route("api/cpu")]
    [ApiController]
    public class CPUMetricController : ControllerBase
    {
        private readonly IMetricsAgentClient _metricsAgentClient;

        public CPUMetricController(IMetricsAgentClient metricsAgentClient)
        {
            _metricsAgentClient = metricsAgentClient;
        }

        [HttpGet("getcpumetrics")]
        [ProducesResponseType(typeof(CpuMetricsResponse), StatusCodes.Status200OK)] //тип объекта, если ответ от сервиса успешный
        public IActionResult GetMetricsFromRequest([FromQuery] CpuMetricsRequest request)
        {
            bool agentStatus = _metricsAgentClient.IsAgentRegisteredAndAvailable(request.AgentId);

            if (!agentStatus) return BadRequest("Агент не зарегистрирован или в статусе \"Отключен\"");

            CpuMetricsResponse response = _metricsAgentClient.GetCpuMetrics(new CpuMetricsRequest()
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
            bool agentStatus = _metricsAgentClient.IsAgentRegisteredAndAvailable(agentId);

            if (!agentStatus) return BadRequest("Агент не зарегистрирован или в статусе \"Отключен\"");

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

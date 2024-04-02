﻿using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi_Manager.Controllers
{
    public class RAMMetricController
    {
        [Route("api/ram")]
        [ApiController]
        public class RAMMetricsController : ControllerBase
        {
            private readonly IMetricsAgentClient _metricsAgentClient;

            public RAMMetricsController(IMetricsAgentClient metricsAgentClient)
            {
                _metricsAgentClient = metricsAgentClient;
            }

            [HttpGet("getRAMMetricsFromAgent")]
            [ProducesResponseType(typeof(RAMMetricsResponse), StatusCodes.Status200OK)] 
            public IActionResult GetMetricsFromAgentV2([FromBody] RAMMetricsRequest request)
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
            public IActionResult GetMetricsFromAgentV1([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
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
}



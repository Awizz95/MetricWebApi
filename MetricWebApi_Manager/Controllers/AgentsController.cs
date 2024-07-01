﻿using MetricWebApi_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricWebApi_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Взаимодействие с агентами")]
    public class AgentsController : ControllerBase
    {
        private readonly AgentPool _agentPool;

        public AgentsController(AgentPool agentPool)
        {
            _agentPool = agentPool;
        }

        [HttpPost("register")]
        [SwaggerOperation(description: "Регистрация нового агента в системе мониторинга")]
        [SwaggerResponse(200, "Успешная операция")]

        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo is not null)
            {
                _agentPool.Add(agentInfo);
            }
            else
            {
                return BadRequest("Ошибка при регистрации агента");
            }

            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        [SwaggerOperation(description: "Изменение стуса агента на \"подключен\"")]
        [SwaggerResponse(200, "Успешная операция")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
            {
                _agentPool.Agents[agentId].Enable = true;
            }
            else
            {
                return BadRequest("Ошибка при изменении статуса агента");
            }

            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        [SwaggerOperation(description: "Изменение стуса агента на \"отключен\"")]
        [SwaggerResponse(200, "Успешная операция")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
            {
                _agentPool.Agents[agentId].Enable = false;
            }
            else
            {
                return BadRequest("Ошибка при изменении статуса агента");
            }

            return Ok();
        }

        [HttpGet("getall")]
        public IActionResult GetAllAgents()
        {
            AgentInfo[] agents = _agentPool.Agents.Values.ToArray();

            return Ok(agents);
        }
    }
}

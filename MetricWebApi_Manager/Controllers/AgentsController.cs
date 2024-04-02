using MetricWebApi_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace MetricWebApi_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Взаимодействие с агентами")]
    public class AgentsController : ControllerBase
    {
        private AgentPool _agentPool;

        public AgentsController(AgentPool agentPool)
        {
            _agentPool = agentPool;
        }

        [HttpPost("register")]
        [SwaggerOperation(description: "Регистрация нового агента в системе мониторинга")]
        [SwaggerResponse(200, "Успешная операция")]

        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            if (agentInfo != null)
            {
                _agentPool.Add(agentInfo);
            }
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        [SwaggerOperation(description: "Изменение стуса агента на \"подключен\"")]
        [SwaggerResponse(200, "Успешная операция")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = true;
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        [SwaggerOperation(description: "Изменение стуса агента на \"отключен\"")]
        [SwaggerResponse(200, "Успешная операция")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            if (_agentPool.Agents.ContainsKey(agentId))
                _agentPool.Agents[agentId].Enable = false;
            return Ok();
        }

        [HttpGet("getall")]
        public IActionResult GetAllAgents()
        {
            return Ok(_agentPool.Agents.Values.ToArray());
        }
    }
}

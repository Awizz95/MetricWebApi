using AutoMapper;
using MetricWebApi.Models.Dto;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CPUMetricController : ControllerBase
    {
        private readonly ICPUMetricRepository _cpuMetricRepository;
        private readonly IMapper _mapper;

        public CPUMetricController(IMapper mapper, ICPUMetricRepository cpuMetricRepository)
        {
            _mapper = mapper;
            _cpuMetricRepository = cpuMetricRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAllCpuMetrics()
        {
            var metrics = _cpuMetricRepository.GetAll();
            var response = new CPUMetricResponse()
            {
                Metrics = new List<CPUMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<CPUMetricDto>(metric));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsForThePeriod ([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = _cpuMetricRepository.GetByTimePeriod(fromTime, toTime);
            var response = new CPUMetricResponse()
            {
                Metrics = new List<CPUMetricDto>()
            };

            foreach (var metric in metrics)
            {
                //вариант без автомаппера
                response.Metrics.Add(new CPUMetricDto
                {
                    Time = TimeSpan.FromSeconds(metric.Time),
                    Value = metric.Value,
                    Id = metric.Id
                });
            }

            return Ok(response);
        }
    }
}

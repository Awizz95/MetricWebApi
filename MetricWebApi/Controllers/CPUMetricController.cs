using AutoMapper;
using MetricWebApi.Models.Dto;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models.Metrics;
using MetricWebApi_Agent.Models.Responses;
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
            IList<CPUMetric> metrics = _cpuMetricRepository.GetAll();

            CPUMetricResponse response = new CPUMetricResponse()
            {
                Metrics = new List<CPUMetricDto>()
            };

            foreach (CPUMetric metric in metrics)
            {
                CPUMetricDto cpuMetricDto = _mapper.Map<CPUMetricDto>(metric);
                response.Metrics.Add(cpuMetricDto);
            }
               
            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetCpuMetricsForThePeriod ([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            IList<CPUMetric> metrics = _cpuMetricRepository.GetByTimePeriod(fromTime, toTime);

            CPUMetricResponse response = new CPUMetricResponse()
            {
                Metrics = new List<CPUMetricDto>()
            };

            foreach (CPUMetric metric in metrics) //вариант без автомаппера
            {
                CPUMetricDto cpuMetricDto = new CPUMetricDto
                {
                    Time = TimeSpan.FromSeconds(metric.Time),
                    Value = metric.Value,
                    Id = metric.Id
                };

                response.Metrics.Add(cpuMetricDto);
            }

            return Ok(response);
        }
    }
}

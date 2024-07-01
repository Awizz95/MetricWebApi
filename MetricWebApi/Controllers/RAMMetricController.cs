using AutoMapper;
using MetricWebApi.Models.Dto;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models.Dto;
using MetricWebApi_Agent.Models.Metrics;
using MetricWebApi_Agent.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MetricWebApi.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RAMMetricController : ControllerBase
    {
        private readonly IRAMMetricRepository _ramMetricRepository;
        private readonly IMapper _mapper;

        public RAMMetricController(IMapper mapper, IRAMMetricRepository ramMetricRepository)
        {
            _mapper = mapper;
            _ramMetricRepository = ramMetricRepository;
        }

        [HttpGet("all")]
        public IActionResult GetAllRAMMetrics()
        {
            IList<RAMMetric> metrics = _ramMetricRepository.GetAll();

            RAMMetricResponse response = new RAMMetricResponse()
            {
                Metrics = new List<RAMMetricDto>()
            };

            foreach (var metric in metrics)
                response.Metrics.Add(_mapper.Map<RAMMetricDto>(metric));

            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetRAMMetricsForThePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            IList<RAMMetric> metrics = _ramMetricRepository.GetByTimePeriod(fromTime, toTime);

            RAMMetricResponse response = new RAMMetricResponse()
            {
                Metrics = new List<RAMMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RAMMetricDto>(metric));
            }

            return Ok(response);
        }
    }
}


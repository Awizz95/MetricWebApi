using AutoMapper;
using MetricWebApi.Models.Dto;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models;
using MetricWebApi_Agent.Models.Dto;
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
            var metrics = _ramMetricRepository.GetAll();
            var response = new RAMMetricResponse()
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
            var metrics = _ramMetricRepository.GetByTimePeriod(fromTime, toTime);
            var response = new RAMMetricResponse()
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


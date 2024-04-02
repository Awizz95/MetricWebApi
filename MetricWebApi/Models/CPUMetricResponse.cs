using MetricWebApi.Models.Dto;

namespace MetricWebApi_Agent.Models
{
    public class CPUMetricResponse
    {
        public List<CPUMetricDto>? Metrics { get; set; }
    }
}

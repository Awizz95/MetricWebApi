using MetricWebApi.Models.Dto;
using MetricWebApi_Agent.Models.Dto;

namespace MetricWebApi_Agent.Models
{
    public class RAMMetricResponse
    {
        public List<RAMMetricDto>? Metrics { get; set; }
    }
}

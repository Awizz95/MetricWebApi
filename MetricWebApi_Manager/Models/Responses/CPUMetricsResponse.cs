using MetricWebApi_Manager.Models.Metrics;

namespace MetricWebApi_Manager.Models.Responses
{
    public class CpuMetricsResponse
    {
        public List<CPUMetric> Metrics { get; set; }
    }
}

using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;

namespace MetricWebApi_Manager.Services.Interfaces
{
    public interface IMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest);
        RAMMetricsResponse GetRAMMetrics(RAMMetricsRequest ramMetricsRequest);
    }
}

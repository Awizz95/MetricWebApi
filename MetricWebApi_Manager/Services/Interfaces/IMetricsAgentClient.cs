using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Models.Responses;

namespace MetricWebApi_Manager.Services.Interfaces
{
    public interface IMetricsAgentClient
    {
        CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest);
        RAMMetricsResponse GetRAMMetrics(RAMMetricsRequest ramMetricsRequest);
        bool IsAgentRegisteredAndAvailable(int agentId);
    }
}

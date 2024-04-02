using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Models;
using MetricWebApi_Manager.Services.Interfaces;
using Newtonsoft.Json;

namespace MetricWebApi_Manager.Services.Implementations
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly AgentPool _agentPool;
        private readonly ILogger<MetricsAgentClient> _logger;

        public MetricsAgentClient(HttpClient httpClient, AgentPool agentPool, ILogger<MetricsAgentClient> logger)
        {

            _logger = logger;
            _httpClient = httpClient;
            _agentPool = agentPool;
        }

        public CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest cpuMetricsRequest)
        {
            try
            {
                AgentInfo? agentInfo = _agentPool.Agents.Values.FirstOrDefault(agent => agent.AgentId == cpuMetricsRequest.AgentId);

                if (agentInfo == null)
                    throw new Exception($"AgentId #{cpuMetricsRequest.AgentId} not found.");

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/cpu/from/{cpuMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{cpuMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse) JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsResponse));
                    cpuMetricsResponse.AgentId = cpuMetricsRequest.AgentId;
                    return cpuMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            throw new Exception("Произошла ошибка запроса");
        }

        public RAMMetricsResponse GetRAMMetrics(RAMMetricsRequest ramMetricsRequest)
        {
            try
            {
                AgentInfo? agentInfo = _agentPool.Agents.Values.FirstOrDefault(agent => agent.AgentId == ramMetricsRequest.AgentId);

                if (agentInfo == null)
                    throw new Exception($"AgentId #{ramMetricsRequest.AgentId} not found.");

                string requestQuery =
                    $"{agentInfo.AgentAddress}api/metrics/ram/from/{ramMetricsRequest.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{ramMetricsRequest.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    RAMMetricsResponse ramMetricsResponse = (RAMMetricsResponse)JsonConvert.DeserializeObject(responseStr, typeof(RAMMetricsResponse));
                    ramMetricsResponse.AgentId = ramMetricsRequest.AgentId;
                    return ramMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            throw new Exception("Произошла ошибка запроса");
        }
    }
}


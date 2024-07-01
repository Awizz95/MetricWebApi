using MetricWebApi_Manager.Models.Request;
using MetricWebApi_Manager.Models.Requests;
using MetricWebApi_Manager.Models;
using MetricWebApi_Manager.Services.Interfaces;
using Newtonsoft.Json;
using MetricWebApi_Manager.Models.Responses;

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

        public CpuMetricsResponse GetCpuMetrics(CpuMetricsRequest request)
        {
            try
            {
                AgentInfo agentInfo = _agentPool.Agents.Values.FirstOrDefault(agent => agent.AgentId == request.AgentId)!;

                Uri agentAddress = agentInfo.AgentAddress;
                string fromTime = request.FromTime.ToString("dd\\.hh\\:mm\\:ss");
                string toTime = request.ToTime.ToString("dd\\.hh\\:mm\\:ss");

                string requestQuery = $"{agentAddress}api/metrics/cpu/from/{fromTime}/to/{toTime}";

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = _httpClient.SendAsync(httpRequestMessage).Result;  //отправка запроса агенту

                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;
                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse) JsonConvert.DeserializeObject(responseStr, typeof(CpuMetricsResponse));
                    cpuMetricsResponse.AgentId = request.AgentId;

                    return cpuMetricsResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            throw new InvalidOperationException("Произошла ошибка запроса");
        }

        public RAMMetricsResponse GetRAMMetrics(RAMMetricsRequest ramMetricsRequest)
        {
            try
            {
                AgentInfo? agentInfo = _agentPool.Agents.Values.FirstOrDefault(agent => agent.AgentId == ramMetricsRequest.AgentId);

                if (agentInfo is null)
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

        public bool IsAgentRegisteredAndAvailable(int agentId)
        {
            bool isAgentRegistered = _agentPool.Agents.Keys.Contains(agentId);

            if (isAgentRegistered)
            {
                bool isAgentAvailable = _agentPool.Agents[agentId].Enable;

                return isAgentAvailable;
            }

            return false;
        }
    }
}


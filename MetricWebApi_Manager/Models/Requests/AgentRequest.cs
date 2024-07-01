namespace MetricWebApi_Manager.Models.Requests
{
    public class AgentRequest
    {
        public required Uri AgentAddress { get; set; }
        public bool Enable { get; set; } = true;
    }
}

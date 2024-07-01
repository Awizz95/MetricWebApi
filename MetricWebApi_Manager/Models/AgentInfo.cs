namespace MetricWebApi_Manager.Models
{
    public class AgentInfo
    {
        public int AgentId { get; set; }
        public required Uri AgentAddress { get; set; }
        public bool Enable { get; set; }
    }
}


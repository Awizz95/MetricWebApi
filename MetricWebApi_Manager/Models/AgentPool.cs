using System.Reflection;

namespace MetricWebApi_Manager.Models
{
    public class AgentPool
    {
        private Dictionary<int, AgentInfo> allAgents;

        public AgentPool()
        {
            allAgents = new Dictionary<int, AgentInfo>();
        }

        public void Add(AgentInfo value)
        {
            if (allAgents.ContainsKey(value.AgentId)) throw new InvalidOperationException("Агент с таким id уже существует");

            allAgents.Add(value.AgentId, value);
        }

        public Dictionary<int, AgentInfo> Agents
        {
            get { return allAgents; }
            set { allAgents = value; }
        }
    }
}

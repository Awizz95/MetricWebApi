namespace MetricWebApi_Manager.Models.Metrics
{
    public class CPUMetric
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
    }
}

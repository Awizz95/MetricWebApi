using MetricWebApi.Models;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models.Metrics;
using Quartz;
using System.Diagnostics;

namespace MetricWebApi_Agent.Jobs
{
    public class RAMMetricJob : IJob
    {

        private readonly IRAMMetricRepository _ramMetricsRepository;
        private readonly PerformanceCounter _ramCounter;

        public RAMMetricJob(IRAMMetricRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        }

        public Task Execute(IJobExecutionContext context)
        {
            float ramValue = _ramCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _ramMetricsRepository.Create(new RAMMetric
            {
                Time = time.TotalSeconds,
                Value = (long)ramValue
            });

            return Task.CompletedTask;
        }
    }
}



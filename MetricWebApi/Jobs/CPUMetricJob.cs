using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models.Metrics;
using Quartz;
using System.Diagnostics;

namespace MetricWebApi.Jobs
{
    public class CpuMetricJob : IJob
    {

        private readonly ICPUMetricRepository _cpuMetricsRepository;
        private readonly PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICPUMetricRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            float cpuUsageInPercents = _cpuCounter.NextValue();
            TimeSpan time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            CPUMetric cpuMetric = new CPUMetric
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            };

            _cpuMetricsRepository.Create(cpuMetric);

            return Task.CompletedTask;
        }
    }
}


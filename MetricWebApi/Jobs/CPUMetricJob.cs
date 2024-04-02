using MetricWebApi.Models;
using MetricWebApi.Services.Interfaces;
using Quartz;
using System.Diagnostics;

namespace MetricWebApi.Jobs
{
    public class CpuMetricJob : IJob
    {

        private readonly ICPUMetricRepository _cpuMetricsRepository;
        private PerformanceCounter _cpuCounter;

        public CpuMetricJob(ICPUMetricRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            float cpuUsageInPercents = _cpuCounter.NextValue();
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _cpuMetricsRepository.Create(new CPUMetric
            {
                Time = time.TotalSeconds,
                Value = (int)cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}


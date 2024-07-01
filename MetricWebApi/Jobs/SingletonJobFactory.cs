using Quartz.Spi;
using Quartz;

namespace MetricWebApi.Jobs
{
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJob? job = _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;

            return job;
        }

        public void ReturnJob(IJob job) { }
    }
}

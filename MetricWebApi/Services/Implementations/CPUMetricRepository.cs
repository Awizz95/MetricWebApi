using Dapper;
using MetricWebApi.Services.Interfaces;
using MetricWebApi_Agent.Models.Metrics;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Data.SQLite;

namespace MetricWebApi.Services.Implementations
{
    public class CPUMetricRepository : ICPUMetricRepository
    {
        private readonly IConfiguration _configuration;

        public CPUMetricRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Create(CPUMetric item)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public void Delete(int id)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            connection.Execute("DELETE FROM cpumetrics WHERE id=@id", new { id = id });
        }

        public void Update(CPUMetric item)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
        }

        public IList<CPUMetric> GetAll()
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            List<CPUMetric> metrics = connection.Query<CPUMetric>("SELECT * FROM cpumetrics").ToList();

            return metrics;
        }

        public CPUMetric GetById(int id)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            CPUMetric metric = connection.QuerySingle<CPUMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id = @id",
                new { id = id });

            return metric;
        }

        public IList<CPUMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            List<CPUMetric> metrics = connection.Query<CPUMetric>($"SELECT * FROM cpumetrics where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();

            return metrics;
        }
    }
}

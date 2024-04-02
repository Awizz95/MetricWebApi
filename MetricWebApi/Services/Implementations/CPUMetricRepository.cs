using Dapper;
using MetricWebApi.Models;
using MetricWebApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Data.SQLite;

namespace MetricWebApi.Services.Implementations
{
    public class CPUMetricRepository : ICPUMetricRepository
    {
        private readonly IOptions<DBOptions> _databaseOptions;

        public CPUMetricRepository(IOptions<DBOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(CPUMetric item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            connection.Execute("DELETE FROM cpumetrics WHERE id=@id", new { id = id });
        }

        public void Update(CPUMetric item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

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
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            List<CPUMetric> metrics = connection.Query<CPUMetric>("SELECT * FROM cpumetrics").ToList();

            return metrics;
        }

        public CPUMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            CPUMetric metric = connection.QuerySingle<CPUMetric>("SELECT Id, Time, Value FROM cpumetrics WHERE id = @id",
                new { id = id });

            return metric;
        }

        public IList<CPUMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            List<CPUMetric> metrics = connection.Query<CPUMetric>($"SELECT * FROM cpumetrics where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();

            return metrics;
        }
    }
}

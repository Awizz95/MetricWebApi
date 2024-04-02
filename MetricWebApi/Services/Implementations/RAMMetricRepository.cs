using MetricWebApi.Services.Interfaces;
using MetricWebApi;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using MetricWebApi.Models;
using Dapper;

namespace MetricWebApi_Agent.Services.Implementations
{
    public class RAMMetricRepository : IRAMMetricRepository
    {
        private readonly IOptions<DBOptions> _databaseOptions;

        public RAMMetricRepository(IOptions<DBOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public void Create(RAMMetric item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public void Delete(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            connection.Execute("DELETE FROM rammetrics WHERE id=@id", new { id = id });
        }

        public void Update(RAMMetric item)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            connection.Execute("UPDATE rammetrics SET value = @value, time = @time WHERE id = @id",
                new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
        }

        public IList<RAMMetric> GetAll()
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            List<RAMMetric> metrics = connection.Query<RAMMetric>("SELECT * FROM rammetrics").ToList();

            return metrics;
        }

        public RAMMetric GetById(int id)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            RAMMetric metric = connection.QuerySingle<RAMMetric>("SELECT Id, Time, Value FROM rammetrics WHERE id = @id",
                new { id = id });

            return metric;
        }

        public IList<RAMMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using var connection = new SQLiteConnection(_databaseOptions.Value.connectionString);

            List<RAMMetric> metrics = connection.Query<RAMMetric>($"SELECT * FROM rammetrics where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();

            return metrics;
        }
    }
}
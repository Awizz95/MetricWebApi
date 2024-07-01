using MetricWebApi.Services.Interfaces;
using MetricWebApi;
using Microsoft.Extensions.Options;
using System.Data.SQLite;
using Dapper;
using MetricWebApi_Agent.Models.Metrics;

namespace MetricWebApi_Agent.Services.Implementations
{
    public class RAMMetricRepository : IRAMMetricRepository
    {
        private readonly IConfiguration _configuration;

        public RAMMetricRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Create(RAMMetric item)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            connection.Execute("INSERT INTO rammetrics(value, time) VALUES(@value, @time)",
                new
                {
                    value = item.Value,
                    time = item.Time
                });
        }

        public void Delete(int id)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            connection.Execute("DELETE FROM rammetrics WHERE id=@id", new { id = id });
        }

        public void Update(RAMMetric item)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

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
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            List<RAMMetric> metrics = connection.Query<RAMMetric>("SELECT * FROM rammetrics").ToList();

            return metrics;
        }

        public RAMMetric GetById(int id)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            RAMMetric metric = connection.QuerySingle<RAMMetric>("SELECT Id, Time, Value FROM rammetrics WHERE id = @id",
                new { id = id });

            return metric;
        }

        public IList<RAMMetric> GetByTimePeriod(TimeSpan timeFrom, TimeSpan timeTo)
        {
            using SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default"));

            List<RAMMetric> metrics = connection.Query<RAMMetric>($"SELECT * FROM rammetrics where time >= @timeFrom and time <= @timeTo",
                new { timeFrom = timeFrom.TotalSeconds, timeTo = timeTo.TotalSeconds }).ToList();

            return metrics;
        }
    }
}
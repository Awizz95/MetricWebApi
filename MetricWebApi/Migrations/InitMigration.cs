using FluentMigrator;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MetricWebApi.Migrations
{
    [Migration(0)]
    public class InitMigration : Migration
    {
        public override void Up()
        {
            Create.Table("cpumetrics").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32().WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("cpumetrics");
        }
    }
}

using FluentMigrator;

namespace MetricWebApi_Agent.Migrations
{
    [Migration(1)]
    public class Migr2 : Migration
    {
        public override void Up()
        {
            Create.Table("rammetrics").WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt64().WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("rammetrics");
        }
    }
}

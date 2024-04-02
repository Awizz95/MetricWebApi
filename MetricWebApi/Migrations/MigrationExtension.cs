using FluentMigrator.Runner;

namespace MetricWebApi.Migrations
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner?.ListMigrations();
            runner?.MigrateUp();
            return app;
        }
    }
}
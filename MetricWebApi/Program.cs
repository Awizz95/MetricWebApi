using MetricWebApi.Converters;
using MetricWebApi.Jobs;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using AutoMapper;
using MetricWebApi;
using MetricWebApi.Services.Interfaces;
using MetricWebApi.Services.Implementations;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using FluentMigrator.Runner;
using MetricWebApi.Migrations;
using System.Reflection;
using MetricWebApi_Agent.Services.Implementations;
using MetricWebApi_Agent.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().
    AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHostedService<QuartzHostedService>();

builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<IJobFactory, SingletonJobFactory>();

builder.Services.AddSingleton(new JobSchedule(typeof(CpuMetricJob), "0/5 * * ? * * *")); //что это?
builder.Services.AddSingleton(new JobSchedule(typeof(RAMMetricJob), "0/5 * * ? * * *"));
builder.Services.AddSingleton<CpuMetricJob>();
builder.Services.AddSingleton<RAMMetricJob>();
builder.Services.AddSingleton<ICPUMetricRepository, CPUMetricRepository>();
builder.Services.AddSingleton<IRAMMetricRepository, RAMMetricRepository>();

builder.Services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSQLite()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                .AddLogging(lb => lb
                    .AddFluentMigratorConsole());

MapperConfiguration mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile())); 
IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MetricsAgent", Version = "v1" });
    c.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("0:00:00:00")
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsAgent v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Migrate();

app.Run();

using MetricWebApi_Manager.Converters;
using MetricWebApi_Manager.Models;
using MetricWebApi_Manager.Services.Implementations;
using MetricWebApi_Manager.Services.Interfaces;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Polly;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(); //регистрируем фабрику IHttpClientFactory

builder.Services.AddHttpClient<IMetricsAgentClient, MetricsAgentClient>()
    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: (attemptCount) => TimeSpan.FromMilliseconds(2000)));

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new CustomTimeSpanConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "API сервиса сбора метрик",
        Description = "API сервиса сбора метрик от агентов",
        TermsOfService = new Uri("https://example.com/terms"), //что это
        Contact = new OpenApiContact
        {
            Name = "Alexander Zhukov",
            Email = "a.zhukov1995@gmail.com",
            Url = new Uri("https://www.google.com"),
        },

        License = new OpenApiLicense
        {
            Name = "Пример лицензии",
            Url = new Uri("https://example.com/license"),
        }
    });

        c.EnableAnnotations(); //Для работы swagger tag
        c.MapType<TimeSpan>(() => new OpenApiSchema // для корректной сериализации типа
        {
            Type = "string",
            Example = new OpenApiString("0:00:00:00")
        });
});

builder.Services.AddSingleton<AgentPool>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MetricsManager v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

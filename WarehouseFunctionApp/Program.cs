using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WarehouseFunctionApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<WarehouseContext>(options =>
        {
            options.UseMySql(configuration["MySqlConnectionString"],
                new MySqlServerVersion(new Version(8, 0, 23)));
        });

        services.AddApplicationInsightsTelemetryWorkerService();
    })
    .Build();

host.Run();

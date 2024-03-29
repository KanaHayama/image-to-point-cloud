﻿using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp {
    public static class Program {
        public static void Main(string[] args) {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.Configure<LimiterOptions>(builder.Configuration);
            builder.Services.AddTransient<Limiter>();
            builder.Services.Configure<ProcessorOptions>(builder.Configuration);
            builder.Services.AddTransient<Processor>();
            builder.Services.Configure<SaverOptions>(builder.Configuration);
            builder.Services.AddTransient<Saver>();
            builder.Services.Configure<ConsoleAppOptions>(builder.Configuration);
            builder.Services.AddHostedService<Worker>();
            var host = builder.Build();
            host.Run();
        }
    }
}

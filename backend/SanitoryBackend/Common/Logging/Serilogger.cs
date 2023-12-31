﻿using System;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging
{
    public static class Serilogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (
                context,
                configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?
                                             .ToLower()
                                             .Replace(".", "-");
                var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

                configuration.WriteTo.Debug()
                             .WriteTo
                             .Console(outputTemplate: "").WriteTo.File("logs/rumble-.txt", rollingInterval: RollingInterval.Minute)
                             .Enrich
                             .FromLogContext()
                             .Enrich
                             .WithMachineName()
                             .Enrich
                             .WithProperty("Environment", environmentName)
                             .Enrich
                             .WithProperty("Application", applicationName)
                             .ReadFrom
                             .Configuration(context.Configuration);
            };
    }
}


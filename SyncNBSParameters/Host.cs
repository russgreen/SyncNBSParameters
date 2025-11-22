using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.GoogleAnalytics;
using SyncNBSParameters.Helpers;
using SyncNBSParameters.Services;
using SyncNBSParameters.ViewModels;
using SyncNBSParameters.Views;
using System;
using System.Globalization;

namespace SyncNBSParameters;
internal static class Host
{
    private static IHost _host;

    public static void StartHost()
    {
        var logPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SyncNBSParameters", "Log.json");
        var cultureInfo = Thread.CurrentThread.CurrentCulture;
        var regionInfo = new RegionInfo(cultureInfo.LCID);
        var clientId = ClientIdProvider.GetOrCreateClientId();

        var loggerConfigSyncNBSParameters = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationVersion", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString())
            .Enrich.WithProperty("RevitVersion", App.CtrApp.VersionNumber)
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Debug(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(new JsonFormatter(), logPath,
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)

            // Local file - exclude usage tracking logs
            .WriteTo.Logger(l => l
                .Filter.ByExcluding(le => le.Properties.ContainsKey("UsageTracking"))
                .WriteTo.File(new JsonFormatter(), logPath,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7));

#if !DEBUG
        //write to google analytics
        loggerConfigSyncNBSParameters = loggerConfigSyncNBSParameters
        .WriteTo.GoogleAnalytics(opts =>
            {
                opts.MeasurementId = "##MEASUREMENTID##";
                opts.ApiSecret = "##APISECRET##";
                opts.ClientId = clientId;

                opts.FlushPeriod = TimeSpan.FromSeconds(1);
                opts.BatchSizeLimit = 1;
                opts.MaxEventsPerRequest = 1;
                opts.IncludePredicate = e => e.Properties.ContainsKey("UsageTracking");

                opts.GlobalParams["app_version"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString();
                opts.GlobalParams["app_country"] = regionInfo.EnglishName;
                opts.GlobalParams["revit_version"] = App.CtrApp.VersionNumber;

                opts.CountryId = regionInfo.TwoLetterISORegionName;
            });
#endif

        Log.Logger = loggerConfigSyncNBSParameters.CreateLogger();

        _host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureServices((_, services) =>
            {
                //services.AddSingleton<ISoftwareUpdateService, SoftwareUpdateService>();

                services.AddTransient<ISettingsService, SettingsService>();

                // ViewModels
                services.AddTransient<ParameterSyncViewModel>();
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<SettingsViewModel>();
            })
            .Build();

        _host.Start();
    }

    public static void StartHost(IHost host)
    {
        _host = host;
        host.Start();
    }

    public static void StopHost()
    {
        _host.StopAsync();
        _host.Dispose();
    }

    public static T GetService<T>() where T : class
    {
        return _host.Services.GetService(typeof(T)) as T;
    }
}

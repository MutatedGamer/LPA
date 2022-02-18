using LPA.Application.AvailableTables;
using LPA.Application.EnabledTables;
using LPA.Application.FileProcessing;
using LPA.Application.PluginLoading;
using LPA.Application.Progress;
using LPA.Application.SDK;
using LPA.Application.Sessions;
using LPA.Common;
using LPA.UI.UserInput;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LPA.Application
{
    public class LpaApplication
    {
        public static void ConfigureServices(string[] args, IHostBuilder builder)
        {
            builder
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<IProgressManager, ProgressManager>();
                    services.AddSingleton<IPluginLoader, PluginLoader>();
                    services.AddSingleton<ISdk, SdkWrapper>();
                    services.AddSingleton<IProgressManager, ProgressManager>();
                    services.AddSingleton<IAvailableTablesManager, AvailableTablesManager>();
                    services.AddSingleton<IEnabledTablesManager, EnabledTablesManager>();
                    services.AddSingleton<IUserInput, UserInput>();
                    services.AddSingleton<IFileProcessor, FileProcessor>();
                    services.AddSingleton<ISessionsManager, SessionsManager>();
                });
        }
    }
}

using Docs_Manager;
using Docs_Manager.Data;
using Docs_Manager.Services;
using Docs_Manager.View;
using Microsoft.Extensions.Logging;

namespace Docs_Manager;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<FileStorageService>();
        builder.Services.AddSingleton<FileShareService>();

        // Pages
        builder.Services.AddTransient<PersonalPage>();
        builder.Services.AddTransient<FilesPage>();

        return builder.Build();
    }
}
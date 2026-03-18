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
        builder.Services.AddTransient<CertificatePage>();
        builder.Services.AddTransient<CocEndorsementPage>();
        builder.Services.AddTransient<DocumentsPage>();
        builder.Services.AddTransient<MedicinePage>();
        builder.Services.AddTransient<OtherPage>();
        builder.Services.AddTransient<ExperiencePage>();
        builder.Services.AddTransient<EditExperiencePage>();
        builder.Services.AddTransient<AddCertificatePage>();
        builder.Services.AddTransient<AddCocPage>();
        builder.Services.AddTransient<AddDocumentPage>();
        builder.Services.AddTransient<AddMedicinePage>();
        builder.Services.AddTransient<AddOtherPage>();
        return builder.Build();
    }
}
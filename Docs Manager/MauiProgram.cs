using Docs_Manager.Data;
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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // ✅ СЕРВИСЫ - ОБЯЗАТЕЛЬНО ДОБАВИТЬ
        builder.Services.AddSingleton<DatabaseService>();

        // ✅ СТРАНИЦЫ
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<PersonalPage>();
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
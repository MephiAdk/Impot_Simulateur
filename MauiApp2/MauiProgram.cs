using MauiApp2.Contract;
using MauiApp2.Service;
using MauiApp2.ViewModels;
using Microsoft.Extensions.Logging;

namespace MauiApp2
{
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
            builder.Services.AddSingleton<IImpotCalculator, ImpotCalculator>();
            builder.Services.AddSingleton<Mainviewmodel>();
            builder.Services.AddSingleton<MainPage>();


            return builder.Build();
        }
    }
}
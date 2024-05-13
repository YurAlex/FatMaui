using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using FatMaui.View;
using FatMaui.ViewModel;

namespace FatMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<NutritionPage>();
            builder.Services.AddSingleton<NutritionViewModel>();

            builder.Services.AddSingleton<ReportsPage>();
            builder.Services.AddSingleton<ReportsViewModel>();

            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddTransient<AddProductViewModel>();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

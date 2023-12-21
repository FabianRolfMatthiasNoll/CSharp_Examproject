using CommunityToolkit.Maui.Storage;
using Frontend.Models;
using Frontend.ViewModel;
using Microsoft.Extensions.Logging;

namespace Frontend {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddDamageViewModel>();
            builder.Services.AddTransient<AddDamagePage>();

            // Register HttpClient
            builder.Services.AddHttpClient();

            // Register ServiceClient
            builder.Services.AddScoped<ServiceClient>(sp => {
                var httpClient = sp.GetRequiredService<HttpClient>();
                return new ServiceClient(@"https://localhost:7095/", httpClient);
            });

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

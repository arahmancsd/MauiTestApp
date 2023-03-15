using CommunityToolkit.Maui;
using MediaManager;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Mopups.Hosting;

namespace MauiTestApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureMopups()
            .RegisterFirebase()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

    private static MauiAppBuilder RegisterFirebase(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) =>
            {
                CrossMediaManager.Current.Init();
                return false;
            }));
#else
            events.AddAndroid(android => android.OnCreate((activity, bundle) => {
                CrossMediaManager.Current.Init();
            }));
#endif
        });

        //builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);
        //builder.Services.AddSingleton(_ => CrossFirebaseStorage.Current);
        //builder.Services.AddSingleton(_ => CrossFirebaseFirestore.Current);
        //builder.Services.AddSingleton(_ => CrossFirebaseCrashlytics.Current);

        return builder;
    }
}


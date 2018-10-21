using System.Globalization;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MoviesMobileApp.Droid
{
    [Activity(Label = "MoviesMobileApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly CultureInfo defaultCulture = new CultureInfo("en-US");

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            var app = new App(new AndroidInitializer());
            LoadApplication(app);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Thread.CurrentThread.CurrentCulture = defaultCulture;
        }
    }
}
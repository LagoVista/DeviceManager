using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using LagoVista.DeviceManager.Droid;

namespace LagoVista.Droid.Simulator
{
    [Activity(Label = "Device Mnager", MainLauncher =true, NoHistory = true, Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenLayout | ConfigChanges.Orientation )]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}
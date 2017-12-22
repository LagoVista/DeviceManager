using System;
using Foundation;
using LagoVista.Core.Models;
using UIKit;


namespace LagoVista.DeviceManager.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public const string MOBILE_CENTER_KEY = "82b1c408-a2bf-42da-b285-eb56719ad2ed"; /* DEV */

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            LagoVista.XPlat.iOS.Startup.Init(app, MOBILE_CENTER_KEY);

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            app.StatusBarHidden = false;

            global::Xamarin.Forms.Forms.Init();
            var formsApp = new App();

            var version = NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
            var versionParts = version.Split('.');
            var versionInfo = new VersionInfo();
            if(versionParts.Length != 4)
            {
                throw new Exception("Expecting CFBundleVersion to be a version conisting of four parts 1.0.218.1231 [Major].[Minor].[Build].[Revision]");
            }
        
            /* if this blows up our build version is borked...make sure all version numbers are intergers like 1.0.218.1231 */
            versionInfo.Major = Convert.ToInt32(versionParts[0]);
            versionInfo.Minor = Convert.ToInt32(versionParts[1]);
            versionInfo.Build = Convert.ToInt32(versionParts[2]);
            versionInfo.Revision = Convert.ToInt32(versionParts[3]);
            formsApp.SetVersionInfo(versionInfo);

            LoadApplication(formsApp);

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            LagoVista.DeviceManager.App.Instance.HandleURIActivation(new Uri(url.AbsoluteString));
            return true;
        }
    }
}

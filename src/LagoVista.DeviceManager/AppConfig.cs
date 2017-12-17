using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagoVista.Core.Models;
using Xamarin.Forms;

namespace LagoVista.DeviceManager
{   
    public class AppConfig : IAppConfig
    {
        public AppConfig()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android: PlatformType = PlatformTypes.Android; break;
                case Device.iOS: PlatformType = PlatformTypes.iPhone; break;
                case Device.UWP: PlatformType = PlatformTypes.WindowsUWP; break;
            }
        }

        public PlatformTypes PlatformType { get; private set; }

        public Environments Environment { get; set; }

        public string WebAddress { get; set; }

        public string AppName => "IoT Device Manager";

        public string AppLogo => "";

        public string CompanyLogo => "";
#if DEBUG
        public bool EmitTestingCode => true;
#else
        public bool EmitTestingCode => true;
#endif

        public string AppId => "D1C45FD985934A40A15FC3322BBCCCCB";
        public string ClientType => "mobileapp";

        public VersionInfo Version { get; set; }
    }
}

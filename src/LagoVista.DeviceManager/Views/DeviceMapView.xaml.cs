using LagoVista.XPlat.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.DeviceManager.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceMapView : LagoVistaContentPage
    {
		public DeviceMapView ()
		{
			InitializeComponent ();
            DevicesMap.GestureRecognizers.Add(new TapGestureRecognizer((view, obj) =>
            {
                var item = obj;

            }));
         
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using APP_KTRA_ROUTER.Droid.Renderer;
using APP_KTRA_ROUTER.Interface;
using Xamarin.Forms;
[assembly: Dependency(typeof(ProcessLoading))]
namespace APP_KTRA_ROUTER.Droid.Renderer
{
    public class ProcessLoading : IProcessLoader
    {
        public async Task Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndroidHUD.AndHUD.Shared.Dismiss();
            });
            
        }

        public async Task Show(string title = "Loading")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndroidHUD.AndHUD.Shared.Show(Forms.Context, status: title, maskType: AndroidHUD.MaskType.Black);
            });

        }
    }
}
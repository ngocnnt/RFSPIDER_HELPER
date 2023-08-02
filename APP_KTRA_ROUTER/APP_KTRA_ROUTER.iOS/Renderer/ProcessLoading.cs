using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using APP_KTRA_ROUTER.Interface;
using BigTed;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(APP_KTRA_ROUTER.iOS.Renderer.ProcessLoading))]
namespace APP_KTRA_ROUTER.iOS.Renderer
{
    public class ProcessLoading : IProcessLoader
    {
        public System.Threading.Tasks.Task Hide()
        {
            BTProgressHUD.Dismiss();
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task Show(string title = "Loading")
        {
            BTProgressHUD.Show(title, maskType: ProgressHUD.MaskType.Black);
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
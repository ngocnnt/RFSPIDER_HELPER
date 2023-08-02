using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.TextInputLayout;
using UIKit;

namespace APP_KTRA_ROUTER.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
           
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();                  
            Syncfusion.SfDataGrid.XForms.iOS.SfDataGridRenderer.Init();
            Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer.Init();
            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfButtonRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfSwitchRenderer.Init();
            Syncfusion.XForms.Pickers.iOS.SfDatePickerRenderer.Init();
            Lottie.Forms.iOS.Renderers.AnimationViewRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            SfMaskedEditRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            new SfNumericTextBoxRenderer();
            Rg.Plugins.Popup.Popup.Init();
            //SQLitePCL.Batteries_V2.Init();
            var libpath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "..", "Library", "data");
            if (!Directory.Exists(libpath))
            {
                Directory.CreateDirectory(libpath);
            }
            var dbpath = Path.Combine(libpath, "rfspider.sqlite");
            LoadApplication(new App(dbpath));

            return base.FinishedLaunching(app, options);
        }
    }
}

using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_KTRA_ROUTER.Services;
using APP_KTRA_ROUTER.Views;
using System.Net.Http;
using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using MQTTnet.Client;
using System.Collections.Generic;
using APP_KTRA_ROUTER.Popup;
using APP_KTRA_ROUTER.Database;
using System.Diagnostics;

namespace APP_KTRA_ROUTER
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;
        public static EMEC_DB_Context gCS_Dbcontext;
        public static string PathDatabase = "";
        [Obsolete]
        public App(string dbPath)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            InitializeComponent();
            new Config();
            PathDatabase = dbPath;
            Debug.WriteLine("$database local at : " + dbPath);
            gCS_Dbcontext = new EMEC_DB_Context(dbPath);
            Device.SetFlags(new[] { "SwipeView_Experimental", "CarouseView_Experimental", "IndicatorView_Experimental" });
            MainPage =  new NavigationPage(new SplashPage ());
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
           
        }

        protected override void OnResume()
        {
           
        }

    }
}

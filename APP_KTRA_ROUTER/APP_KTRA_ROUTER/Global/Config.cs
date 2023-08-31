using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Global
{
    public class Config 
    {
        public static string URL = "";
        public static string TOPIC = "topic";
        public static string User = "User";
        public static string Password = "Password";
        public static string Token { get; set; }
        public static string DonVi = "DonVi";
        public static System.Net.Http.HttpClient client;
        public static IMessagingCenter messagingCenter;
        public static readonly string urlService = "UrlService";
        public static readonly string onLine = "onLine";
        public static readonly string offLine = "offLine";
        public static readonly string maDonVi = "maDonVi";
        public static readonly string canDuoi = "canDuoi";
        public static readonly string canTren = "canTren";
        public static readonly string bluetooth = "bluetooth";
        public static readonly string ky = "ky";
        public static readonly string thang = "thang";
        public static readonly string nam = "nam";
        public Config()
        {
            //if (Device.RuntimePlatform == Device.iOS)
            //{ 
            //    URL = "http://113.160.225.75:80/API_DCU_ROUTER/";
            //}    
            //else
            {
                URL = "http://smart.cpc.vn/DCU_ROUTER/";
            }    
                
            if (client == null)
            {
                client = new System.Net.Http.HttpClient();
            }
            messagingCenter = MessagingCenter.Instance;
        }


    }

    public enum DialogReturn
    {
        OK = 0,
        Cancel = 1,
        Repeat = 2,
        Stop = 3
    }
}

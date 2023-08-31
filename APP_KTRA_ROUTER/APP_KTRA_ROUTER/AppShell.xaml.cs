using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Popup;
using APP_KTRA_ROUTER.Views;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace APP_KTRA_ROUTER
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        MqttClientRepository repository = new MqttClientRepository();
        public AppShell()
        {
            InitializeComponent();
            //đăng ki kênh mqtt       
            if (MqttClientRepository.client == null)
            {
                MqttClientRepository.client = repository.Create("222.255.138.213", 1883, "lucnv", "lucnv", new List<string> { "REQUEST/CPC/PC01/#", "REQUEST/CPC/PC02/#", "REQUEST/CPC/PC03/#"
                ,"REQUEST/CPC/PP/#", "REQUEST/CPC/PC05/#", "REQUEST/CPC/PC06/#","REQUEST/CPC/PC07/#", "REQUEST/CPC/PC08/#", "REQUEST/CPC/PQ/#"
                , "REQUEST/CPC/PC10/#","REQUEST/CPC/PC11/#", "REQUEST/CPC/PC12/#", "REQUEST/CPC/PC13/#"}, Guid.NewGuid().ToString());//
            }
        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS )
            App.Current.MainPage = new Login();
        }

        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            await new AppInformation().Show();
        }

        private void MenuItem_Clicked_2(object sender, EventArgs e)
        {

        }

        private void MenuItem_Clicked_3(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ThietBiDenHanKiemDinh());
        }
    }
}

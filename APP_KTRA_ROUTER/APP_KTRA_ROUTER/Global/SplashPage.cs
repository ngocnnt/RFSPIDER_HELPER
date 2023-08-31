using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Popup;
using APP_KTRA_ROUTER.Views;
using Lottie.Forms;
using MQTTnet.Client;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Global
{
    public class SplashPage : ContentPage
    {
       // MqttClientRepository repository = new MqttClientRepository();
       // IMqttClient client;
        Image image;
        AnimationView animation;
        public SplashPage()
        {


            if (Device.RuntimePlatform == Device.iOS)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                var sub = new AbsoluteLayout();
                animation = new AnimationView
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Animation = "working.json",
                    AutoPlay = true,
                    Loop = true,
                    Speed = float.Parse("0.5")
                };
                animation.Play();
                //image = new Image
                //{
                //    Source = "logo.png",
                //    WidthRequest = 300,
                //    HeightRequest = 300

                //};
                AbsoluteLayout.SetLayoutFlags(animation, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(animation, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                sub.Children.Add(animation);
                this.BackgroundColor = Color.White;
                this.Content = sub;
            }


        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            if (!CrossConnectivity.Current.IsConnected)
            {
                await ShowMessage("Thông Báo", "Vui Lòng kiểm tra lại kết nối mạng", "OK", () =>
                { App.Current.MainPage = new Login(); });
            }
            if (Device.RuntimePlatform == Device.iOS)
            {
                

                //await image.ScaleTo(1, 2000);//thời gian khởi tạo
                //await image.ScaleTo(0.9, 700, Easing.Linear);
                //await image.ScaleTo(150, 300, Easing.Linear);

            }
            //kiêm tra xem user có thay đổi k
            try
            {              


                //MqttClientRepository.client  = repository.Create("113.160.225.75", 1883, "lucnv", "lucnv", new List<string> { "RFSPIDER_RECEIVE" }, Guid.NewGuid().ToString ());
               
                
                if (Device.RuntimePlatform == Device.iOS)
                {
                    Preferences.Set(Config.User, "appios");
                    App.Current.MainPage = new AppShell();
                }
                else
                {
                    var response = Config.client.GetStringAsync(Config.URL + "api/home/GetUserAD?username=" + Preferences.Get(Config.User, "1") + "&password=" + Preferences.Get(Config.Password, "1")).Result;
                    if (response == "false")
                        App.Current.MainPage = new Login();
                    else
                        App.Current.MainPage = new AppShell();
                }    

            }
            catch (Exception ex)
            {

                App.Current.MainPage = new Login();
            }


        }

        private  async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
           if (e.IsConnected == true )
            {
                try
                {
                    await MqttClientRepository.client.ReconnectAsync();
                    //if (Device.RuntimePlatform != Device.iOS )
                    //await new MessageBox("Thông Báo", "Bạn đã kết nối lại với server").Show();
                }
                catch 
                {
                   
                }
               
            }    
        }

        public async Task ShowMessage(string title, string message,  string buttonText, Action afterHideCallback)
        {
            await DisplayAlert(
                title,
                message,
                buttonText);

            afterHideCallback?.Invoke();
        }
    }
}

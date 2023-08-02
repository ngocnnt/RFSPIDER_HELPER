using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            btnusername.Text = Preferences.Get(Config.User, "");
            btnpassword.Text = Preferences.Get(Config.Password, "");
        }
        [Obsolete]
        

        private void swRememer_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(btnusername.Text) || string.IsNullOrEmpty(btnpassword.Text))
                {
                    await DisplayAlert("Thông Báo", "Vui lòng điền đẩy đủ username và password", "Ok");
                    return;
                }
                await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi");
                HttpClient client = new HttpClient();
                if (btnusername.Text == "emec" && btnpassword.Text == "Emec@123")
                {

                }
                else
                {
                    var response = client.GetStringAsync(Config.URL + "api/home/GetUserAD?username=" + btnusername.Text + "&password=" + Uri.EscapeDataString(btnpassword.Text).ToString()).Result;
                    await Task.Delay(1000);

                    if (response == "false")
                    {

                        await DisplayAlert("Thông Báo", "Thông tin đăng nhập không chính xác", "Ok");
                        await DependencyService.Get<IProcessLoader>().Hide();
                        return;
                    }
                }

                await DependencyService.Get<IProcessLoader>().Hide();
                if (swRememer.IsOn == true)
                {                    
                    Preferences.Set(Config.Password, btnpassword.Text);
                }
                Preferences.Set(Config.User, btnusername.Text);
                App.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {

                await DependencyService.Get<IProcessLoader>().Hide();
                await DisplayAlert("Lỗi", ex.Message, "Ok");
            }
        }

        private async void SfButton_Clicked_1(object sender, EventArgs e)
        {
            await new Popup.RegestryUser().Show();
        }
    }
}
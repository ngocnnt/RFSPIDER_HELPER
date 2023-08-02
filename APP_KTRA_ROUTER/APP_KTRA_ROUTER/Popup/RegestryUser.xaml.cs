using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegestryUser : PopupPage
    {
        public RegestryUser()
        {
            InitializeComponent();
        }
        TaskCompletionSource<DialogReturn> _tsk = null;
        
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            if (btnpassword.Text != btnrepassword.Text )
            {
                await new MessageBox("Thông báo", "Mật khẩu không giống nhau. Vui lòng kiểm tra lại").Show();
                return;
            }
            await DependencyService.Get<IProcessLoader>().Show("Đang tạo user");
            HttpClient client = new HttpClient();

            var response = client.GetStringAsync(Config.URL + "api/home/RegisterUser?username=" + btnusername.Text + "&password=" + btnpassword.Text).Result;
            await Task.Delay(3000);
            if (response == "false")
            {

                await DisplayAlert("Thông Báo", "User này đã tồn tại", "Ok");
                await DependencyService.Get<IProcessLoader>().Hide();
                return;
            }
            await DisplayAlert("Thông Báo", "Đăng ký thành công. vui lòng đăng nhập để tiếp tục", "Ok");
            await Navigation.PopAllPopupAsync();
            _tsk.SetResult(DialogReturn.OK);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}
using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Popup;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageXacThucMatKhau : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;
        string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private bool _isShowHidePassword;
        public bool IsPasswordShow
        {
            get => _isShowHidePassword;
            set
            {
                _isShowHidePassword = value;
                OnPropertyChanged("IsPasswordShow");
            }
        }
        public Command ShowHideTapCommand { get; }
        public Command XacThucCommand { get; }
        public Command CloseCommand { get; }
        public MessageXacThucMatKhau()
        {
            InitializeComponent();
            _isShowHidePassword = true;
            XacThucCommand = new Command(OnXacThucClicked, ValidateLogin);
            this.PropertyChanged +=
                                  (_, __) => XacThucCommand.ChangeCanExecute();
            CloseCommand = new Command(OnCloseClicked);
            BindingContext = this;
        }

        private async void OnCloseClicked(object obj)
        {
            await Navigation.PopPopupAsync();
            _tsk.SetResult(DialogReturn.Cancel);
        }

        private async void OnXacThucClicked()
        {
            try
            {
                await Task.Delay(200);                
                if (Password == "41411414")
                {
                    await Navigation.PopPopupAsync();
                    _tsk.SetResult(DialogReturn.OK);
                }
                else
                {
                    await new MessageBox("Thông báo", "Sai mật khẩu. Vui lòng kiểm tra lại").Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        private bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(_password);
        }

        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

        private void imgShowPassword_Tapped(object sender, EventArgs e)
        {
            IsPasswordShow = !IsPasswordShow;
        }
    }
}
using APP_KTRA_ROUTER.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageYESNO : PopupPage
    {
        TaskCompletionSource<DialogReturn> _tsk = null;

        public MessageYESNO(string thongbao, string noidung)
        {
            InitializeComponent();
            lblThongBao.Text = thongbao;
            lblMessage.Text = noidung;
        }

        private async void btnOK_Clicked(object sender, EventArgs e)
        {

            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(DialogReturn.OK);
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(DialogReturn.Cancel);
        }
        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}
using APP_KTRA_ROUTER.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CBox_DienLuc : PopupPage
    {
        TaskCompletionSource<DonVi> _tsk = null;
        public ObservableCollection<DonVi> _listDonVi { get; set; }
        public CBox_DienLuc(ObservableCollection<DonVi> listDonvi)
        {
            InitializeComponent();
            listviewDonVi.ItemsSource = listDonvi;
        }

        private async  void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DonVi donVi = e.SelectedItem as DonVi;
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(donVi );
        }
        public async Task<DonVi > Show()
        {
            _tsk = new TaskCompletionSource<DonVi>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
    }
}
using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dcu_History : ContentPage
    {
        string ma_dien_luc = "";
        string ma_tram = "";      
        ObservableCollection<DCU_ROUTER> _lstDCU { get; set; }
        APP_KTRA_ROUTER.ViewModels.Dcu_HistoryViewModel viewModel;
        public Dcu_History() 
        
        {
            InitializeComponent();

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
           {
              Navigation.PushAsync(new Setting());
            }
            BindingContext = viewModel = new ViewModels.Dcu_HistoryViewModel();
        }        
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string serial = search.Text;
            if (serial != "")
            {
                listviewDCU.ItemsSource = viewModel.LstDcuRouter.Where(p => p.DcuID.ToLower().Contains(serial.ToLower())).ToList();
            }
            else
            {
                listviewDCU.ItemsSource = viewModel.LstDcuRouter;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.DonVis.Count == 0 )
            viewModel.LoadDonvisCommand.Execute(null);
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            BackButtonPressed();
            return true;
        }
        public async Task BackButtonPressed()
        {
            var ok = await DisplayAlert("Thông báo", "Bạn có muốn thoát chương trình không?", "ok", "cancle");
            if (ok)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }
    }
   
}
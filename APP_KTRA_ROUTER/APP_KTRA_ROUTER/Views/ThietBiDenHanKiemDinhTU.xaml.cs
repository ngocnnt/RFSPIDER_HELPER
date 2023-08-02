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
    public partial class ThietBiDenHanKiemDinhTU : ContentPage
    {
        string ma_dien_luc = "";
        string ma_tram = "";
        ObservableCollection<DCU_ROUTER> _lstDCU { get; set; }
        APP_KTRA_ROUTER.ViewModels.ThietBiDenHanKiemDinhViewModel viewModel;
        public ThietBiDenHanKiemDinhTU()

        {
            InitializeComponent();

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
            {
                Navigation.PushAsync(new Setting());
            }
            BindingContext = viewModel = new ViewModels.ThietBiDenHanKiemDinhViewModel();
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string serial = search.Text;
            if (serial != "")
            {
                listviewDCU.ItemsSource = viewModel.LstDcuRouter.Where(p => p.SO_TBI.ToLower().Contains(serial.ToLower())).ToList();
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
            if (viewModel.DonVis.Count == 0)
                viewModel.LoadDonvisCommand.Execute(null);
            viewModel.LoadNamsCommand.Execute(null);
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
        private async void btnLayDS_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Điện lực").Show();
                    return;
                }
                if (cbThang.SelectedItem == null || cbThang.SelectedItem.ToString() == "" || cbNam.SelectedItem == null || cbNam.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn tháng, năm").Show();
                    return;
                }
                viewModel.LoadDCUCommand.Execute(((DonVi)cbDienLuc.SelectedItem).MA_DON_VI + "/TU/" + cbThang.SelectedItem.ToString() + "/" + cbNam.SelectedItem.ToString());
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }
        private async void btnThao_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(viewModel.LstDcuRouter.Where(x=>x.SO_TBI == search.Text).ToList().Count() != 1)
                {
                    await new MessageBox("thông báo", "Vui lòng nhập đúng số thiết bị có trong danh sách.").Show();
                } else
                {
                    if (viewModel.LstDcuRouter.Where(x => x.SO_TBI == search.Text).ToList()[0].DA_THAO == true)
                    {
                        await new MessageBox("thông báo", "Thiết bị đã tháo.").Show();
                    }
                    else
                    {
                        var result = await this.DisplayAlert("Xác nhận!", "Tháo thiết bị?", "Yes", "No");
                        if (result)
                        {
                            TTinKDinh item = viewModel.LstDcuRouter.Where(x => x.SO_TBI == search.Text).ToList()[0];
                            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                            {
                                var _json = Config.client.PostAsync(Config.URL + "api/home/INSUPD_TBI_DHAN_KDINH?ma_dviqly=" + item.MA_DVIQLY +
                                     "&ma_ddo=" + item.MA_DDO + "&loai_tbi=TU" + "&ma_tbi=" + item.MA_TBI + "&so_tbi=" + item.SO_TBI + "&ma_cloai=" + item.MA_CLOAI +
                                     "&ngay_kdinh=" + item.NGAY_KDINH.ToString("yyyy-MM-dd") + "&han_kdinh=" + item.HAN_KDINH.ToString("yyyy-MM-dd") + "&ngay_capnhat=" + DateTime.Now.ToString("yyyy-MM-dd") + "&nguoi_capnhat=" + Xamarin.Essentials.Preferences.Get(Config.User, ""), null).Result;
                                var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();

                                if (content == "true")
                                {
                                    await new MessageBox("Thông Báo", "Tháo thành công").Show();
                                    viewModel.LstDcuRouter.Where(x => x.SO_TBI == search.Text).ToList()[0].DA_THAO = true;
                                    viewModel.LstDcuRouter.Where(x => x.SO_TBI == search.Text).ToList()[0].NOT_DA_THAO = false;
                                    search.Text = "";
                                    if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                                    {
                                        await new MessageBox("thông báo", "Vui lòng chọn Điện lực").Show();
                                        return;
                                    }
                                    if (cbThang.SelectedItem == null || cbThang.SelectedItem.ToString() == "" || cbNam.SelectedItem == null || cbNam.SelectedItem.ToString() == "")
                                    {
                                        await new MessageBox("thông báo", "Vui lòng chọn tháng, năm").Show();
                                        return;
                                    }
                                }
                                else
                                {
                                    await new MessageBox("Thông Báo", "Xảy ra lỗi trong quá trình cập nhật.").Show();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }
        private async void scanBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    search.Text = result.Text;
                });

            };
        }
    }
}
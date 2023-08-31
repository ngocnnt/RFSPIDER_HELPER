using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
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
    public partial class TreothaoDcuRouter : ContentPage
    {
        string ma_dien_luc = "";
        string ma_tram = "";
        ObservableCollection<DCU_ROUTER> _lstDCU { get; set; }
        APP_KTRA_ROUTER.ViewModels.TreoThaoDcuRouterViewModel viewModel;
        public TreothaoDcuRouter()

        {
            InitializeComponent();

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
            {
                Navigation.PushAsync(new Setting());
            }
            BindingContext = viewModel = new ViewModels.TreoThaoDcuRouterViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.DonVis.Count == 0)
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

        private void cbThaoTac_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            myGrid.RowDefinitions[2].Height = 50;
            if (((ThaoTac)cbThaoTac.SelectedItem).LOAI_THAO_TAC == "TT")
            {
                IdCu.IsVisible = true;
                IdMoi.IsVisible = true;
                IdThao.IsVisible = false;
                IdLapMoi.IsVisible = false;
                cbTram.IsVisible = false;
                cbDienLuc.IsVisible = false;
                cbLoaiTram.IsVisible = false;
                cbLyDoThao.IsVisible = false;
                btnLapMoi.IsVisible = false;
                btnThayThe.IsVisible = true;
                btnThao.IsVisible = false;
                txtMaTru.IsVisible = false;
                txtMaLo.IsVisible = false;
                txtMoTa.IsVisible = false;
                txtMoTaThayThe.IsVisible = true; 
                if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                {
                    cbBienDong.IsVisible = true;
                } else
                {
                    cbBienDong.IsVisible = false;
                }
            }
            else if(((ThaoTac)cbThaoTac.SelectedItem).LOAI_THAO_TAC == "LM")
            {
                IdCu.IsVisible = false;
                IdMoi.IsVisible = false;
                cbTram.IsVisible = true;
                cbDienLuc.IsVisible = true;
                cbLoaiTram.IsVisible = true;
                IdThao.IsVisible = false;
                IdLapMoi.IsVisible = true;
                cbLyDoThao.IsVisible = false;
                btnLapMoi.IsVisible = true;
                btnThayThe.IsVisible = false;
                btnThao.IsVisible = false;
                txtMaTru.IsVisible = false;
                txtMaLo.IsVisible = false;
                txtMoTa.IsVisible = false;
                txtMoTaThayThe.IsVisible = false;
                cbBienDong.IsVisible = false;
                if (cbLoaiThietBi.SelectedItem.ToString() == "Router")
                {
                    txtMaTru.IsVisible = true;
                    txtMaLo.IsVisible = true;
                    txtMoTa.IsVisible = true;
                }
            }
            if (((ThaoTac)cbThaoTac.SelectedItem).LOAI_THAO_TAC == "TH")
            {
                IdCu.IsVisible = false;
                IdMoi.IsVisible = false;
                cbTram.IsVisible = false;
                cbDienLuc.IsVisible = false;
                cbLoaiTram.IsVisible = false;
                IdThao.IsVisible = true;
                IdLapMoi.IsVisible = false;
                cbLyDoThao.IsVisible = true;
                btnLapMoi.IsVisible = false;
                btnThayThe.IsVisible = false;
                btnThao.IsVisible = true;
                txtMaTru.IsVisible = false;
                txtMaLo.IsVisible = false;
                txtMoTa.IsVisible = false;
                txtMoTaThayThe.IsVisible = false;
                cbBienDong.IsVisible = false;
            }
        }

        private async void scanIdThaoBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    IdThaoText.Text = result.Text;
                });

            };
        }

        private async void scanIdCuBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    IdCuText.Text = result.Text;
                });

            };
        }

        private async void scanIdMoiBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    IdMoiText.Text = result.Text;
                });

            };
        }

        private async void scanIdLapMoiBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    IdLapMoiText.Text = result.Text;
                });

            };
        }

        private async void cbDienLuc_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbLoaiThietBi.SelectedItem == null || cbLoaiThietBi.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Thiết bị DCU/Router").Show();
                    return;
                }
                if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                {
                    if (cbLoaiTram.SelectedItem == null || cbLoaiTram.SelectedItem.ToString() == "")
                    {
                        await new MessageBox("thông báo", "Vui lòng chọn Trạm chuyên dùng/công cộng").Show();
                        return;
                    }
                    if (cbLoaiTram.SelectedValue.ToString() == "Công cộng")
                        viewModel.LoadTramsCCChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                    else
                        viewModel.LoadTramsCDChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                } else
                {
                    viewModel.LoadTramsCommand.Execute(viewModel.SelectItemDonVi);
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private async void cbLoaiThietBi_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                if (((ThaoTac)cbThaoTac.SelectedItem).LOAI_THAO_TAC == "LM")
                {
                    if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                    {
                        cbLoaiTram.IsVisible = true;

                        txtMaTru.IsVisible = false;
                        txtMaLo.IsVisible = false;
                        txtMoTa.IsVisible = false;
                        myGrid.RowDefinitions[2].Height = 50;
                        if (cbLoaiTram.SelectedItem == null || cbLoaiTram.SelectedItem.ToString() == "")
                        {
                            return;
                        }
                        if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                        {
                            return;
                        }
                        if (cbLoaiTram.SelectedValue.ToString() == "Công cộng")
                            viewModel.LoadTramsCCChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                        else
                            viewModel.LoadTramsCDChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                    }
                    else
                    {
                        cbLoaiTram.IsVisible = false;

                        txtMaTru.IsVisible = true;
                        txtMaLo.IsVisible = true;
                        txtMoTa.IsVisible = true;
                        myGrid.RowDefinitions[2].Height = 0;
                        if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                        {
                            return;
                        }
                        viewModel.LoadTramsCommand.Execute(viewModel.SelectItemDonVi);
                    }
                } else
                {
                    cbLoaiTram.IsVisible = false;
                    myGrid.RowDefinitions[2].Height = 50;
                    if (((ThaoTac)cbThaoTac.SelectedItem).LOAI_THAO_TAC == "TT")
                    {
                        txtMoTaThayThe.IsVisible = true;
                        if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                        {
                            cbBienDong.IsVisible = true;
                        } else
                        {
                            cbBienDong.IsVisible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private async void cbLoaiTram_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbLoaiThietBi.SelectedItem == null || cbLoaiThietBi.SelectedItem.ToString() == "")
                {
                    return;
                }
                if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                {
                    if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                    {
                        return;
                    }
                    if (cbLoaiTram.SelectedValue.ToString() == "Công cộng")
                        viewModel.LoadTramsCCChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                    else
                        viewModel.LoadTramsCDChuaKBaoCommand.Execute(viewModel.SelectItemDonVi);
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private async void btnLapMoi_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (cbLoaiThietBi.SelectedItem == null || cbLoaiThietBi.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Thiết bị DCU/Router").Show();
                    return;
                }
                if (cbDienLuc.SelectedItem == null || cbDienLuc.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Điện lực").Show();
                    return;
                }
                if (cbTram.SelectedItem == null || cbTram.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Trạm").Show();
                    return;
                }
                if (IdLapMoiText.Text == null || IdLapMoiText.Text == "")
                {
                    await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã ID DCU/Router").Show();
                    return;
                }


                if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                {
                    if (cbLoaiTram.SelectedItem == null || cbLoaiTram.SelectedItem.ToString() == "")
                    {
                        await new MessageBox("thông báo", "Vui lòng chọn Trạm chuyên dùng/Công cộng").Show();
                        return;
                    }
                    var loaiTram = "";
                    if (cbLoaiTram.SelectedItem.ToString() == "Công cộng")
                    {
                        loaiTram = "CC";
                    }
                    else
                    {
                        loaiTram = "CD";
                    }
                    if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                    {
                        var _json = Config.client.PostAsync(Config.URL + "api/home/KBAO_DCU?donvi=" + ((TRAM)cbTram.SelectedItem).MA_DVIQLY +
                             "&user=" + Xamarin.Essentials.Preferences.Get(Config.User, "") + "&ma_dien_luc=" + ((TRAM)cbTram.SelectedItem).MA_DVIQLY + 
                             "&ma_tram=" + ((TRAM)cbTram.SelectedItem).MA_TRAM  + "&ten_tram=" + ((TRAM)cbTram.SelectedItem).TEN_TRAM +
                             "&dia_chi=" + ((TRAM)cbTram.SelectedItem).DIA_CHI + "&loai_tram=" + loaiTram + "&id_dcu=" + IdLapMoiText.Text,null).Result;
                        var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "");

                        await new MessageBox("Thông Báo", content).Show();
                        DCU_ROUTER dCU = new DCU_ROUTER();
                        dCU.DcuID = IdLapMoiText.Text;
                        await Navigation.PushAsync(new Dcu_Check(IdLapMoiText.Text, ((TRAM)cbTram.SelectedItem).MA_TRAM, ((TRAM)cbTram.SelectedItem).MA_DVIQLY, dCU));
                    }
                }
                else
                {
                    if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                    {
                        var _json = Config.client.PostAsync(Config.URL + "api/home/KBAO_ROUTER?donvi=" + ((TRAM)cbTram.SelectedItem).MA_DVIQLY +
                             "&user=" + Xamarin.Essentials.Preferences.Get(Config.User, "") + "&seryrouter=" + IdLapMoiText.Text +
                             "&ma_tram=" + ((TRAM)cbTram.SelectedItem).MA_TRAM + "&malo=" + MaLoText.Text + "&matru=" + MaTruText.Text + "&detail=" + MoTaText.Text, null).Result;
                        var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "");

                        await new MessageBox("Thông Báo", content).Show();
                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private async void btnThayThe_Clicked(object sender, EventArgs e)
        {
            await new MessageBox("thông báo", "Chức năng thay thế tạm thời không thể sử dụng.").Show();
            return;
            try
            {
                if (cbLoaiThietBi.SelectedItem == null || cbLoaiThietBi.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn Thiết bị DCU/Router").Show();
                    return;
                }
                if (IdMoiText.Text == null || IdMoiText.Text == "" || IdCuText.Text == null || IdCuText.Text == "")
                {
                    await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã ID DCU/Router").Show();
                    return;
                }

                if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
                {
                    
                    if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                    {
                        var _json = Config.client.PostAsync(Config.URL + "api/home/THAYTHE_DCU?donvi=" + Xamarin.Essentials.Preferences.Get(Config.maDonVi, "") +
                             "&user=" + Xamarin.Essentials.Preferences.Get(Config.User, "") + "&bdongdcu=" + (cbBienDong.SelectedItem.ToString() == "Tháo" ? "THAO" : "HONG") +
                             "&mota=" + MoTaThayTheText.Text + "&id_dcu_new=" + IdMoiText.Text + "&id_dcu_old=" + IdCuText.Text, null).Result;
                        var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "");

                        await new MessageBox("Thông Báo", content).Show();
                        
                    }
                } else
                {
                    if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                    {
                        var _json = Config.client.PostAsync(Config.URL + "api/home/THAYTHE_ROUTER?donvi=" + Xamarin.Essentials.Preferences.Get(Config.maDonVi, "") +
                             "&user=" + Xamarin.Essentials.Preferences.Get(Config.User, "") + 
                             "&seryrouter=" + IdMoiText.Text + "&oldsery=" + IdCuText.Text + "&detail=" + MoTaThayTheText.Text, null).Result;
                        var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "");

                        await new MessageBox("Thông Báo", content).Show();

                    }
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private async void btnThao_Clicked(object sender, EventArgs e)
        {
            if (cbLoaiThietBi.SelectedItem == null || cbLoaiThietBi.SelectedItem.ToString() == "")
            {
                await new MessageBox("thông báo", "Vui lòng chọn Thiết bị DCU/Router").Show();
                return;
            }

            if (IdThaoText.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã ID DCU/Router").Show();
                return;
            }
            if (cbLoaiThietBi.SelectedItem.ToString() == "DCU")
            {
                await new MessageBox("Thông Báo", "Chưa sử dụng được chức năng tháo DCU").Show();
            } else
            {
                if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                {
                    var _json = Config.client.PostAsync(Config.URL + "api/home/XOA_ROUTER?donvi=" + Xamarin.Essentials.Preferences.Get(Config.maDonVi, "") +
                         "&user=" + Xamarin.Essentials.Preferences.Get(Config.User, "") +
                         "&seryrouter=" + IdThaoText.Text + "&detail=" + LyDoThaoText.Text, null).Result;
                    var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "");

                    await new MessageBox("Thông Báo", content).Show();

                }
            }
        }
    }
}
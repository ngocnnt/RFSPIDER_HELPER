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
    public partial class MainPage : ContentPage
    {
        string filterText = "";        
        ObservableCollection<DCU_ROUTER> _lstDCU { get; set; }
        APP_KTRA_ROUTER.ViewModels.MainViewModel viewModel;
        public MainPage()
        
        {
            InitializeComponent();

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
            {
              Navigation.PushAsync(new Setting());
            }
            BindingContext = viewModel = new ViewModels.MainViewModel();
            MessagingCenter.Subscribe<SubscribeCallback, DcuMqttResp>(this, "MQTT", (obj, item) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    try
                    {
                        if (item.TenDangNhap != Preferences.Get(Config.User, "")) return;
                        if (item.TypeReq == null) return;
                        if (item.MaTram != viewModel.SelectItemTram.MA_TRAM) return;
                        if (item.TypeReq.ToLower() == "sys" || item.TypeReq.ToLower() == "readtram")
                        {
                            DependencyService.Get<IMessage>().ShortAlert(item.ErrorCode );
                            viewModel.LoadDCUCommand.Execute(viewModel.SelectItemTram);
                        }
                        
                    }
                    catch
                    {
                    }


                });
            });
        }
        public bool FilterRecords(object o)
        {

            var item = o as DCU_ROUTER;

            if (item != null)
            {

                if (item.MeterID.ToLower().Contains(filterText) || item.TEN_TRAM.ToLower().Contains(filterText) )
                    return true;
            }
            return false;
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            listviewDCU.View.Filter = FilterRecords;
            listviewDCU.View.RefreshFilter();
        }

        private async void btnGuiYeuCau_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                DCU_ROUTER dCU = listviewDCU.SelectedItem as DCU_ROUTER;
                if (dCU != null)
                {
                    if (dCU.Type == "DCU")
                    {
                        await Navigation.PushAsync(new Dcu_Check(dCU.DcuID, viewModel.SelectItemTram.MA_TRAM , viewModel.SelectItemDonVi.MA_DON_VI , dCU));
                    }

                    else
                    {
                        await Navigation.PushAsync(new CongTo_Check(viewModel.SelectItemTram.MA_TRAM , viewModel.SelectItemDonVi.MA_DON_VI , dCU));
                    }

                }

            }
            catch (Exception ex)
            {
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
        }

        private async void btnScan_Clicked_1(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            scan.Title = "Quét barcode công tơ";
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (ketqua) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    string str = Config.URL + "api/home/TIM_CONG_TO?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&serial=" + ketqua.Text ;
                    var _json = Config.client.GetStringAsync(str).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        ObservableCollection<TIM_CONG_TO> TimCongTos = JsonConvert.DeserializeObject<ObservableCollection<TIM_CONG_TO>>(result);
                        foreach (TIM_CONG_TO cto in TimCongTos)
                        {
                            DCU_ROUTER dCU = new DCU_ROUTER { DcuID = cto.ID_DCU, MeterID = cto.SERY_CTO, Type = cto.METER_TYPE };
                            await Navigation.PushAsync(new CongTo_Check(cto.MA_TRAM , cto.MA_DVIQLY, dCU));
                        }
                    }
                    else
                    {
                        await new MessageBox("Thông báo", "Không tìm thấy số Sery : " + ketqua.Text + " trong hệ thống").Show();
                        return;
                    }    
                  
                });

            };
        }

        private async void btnChiDuong_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                DCU_ROUTER dCU = listviewDCU.SelectedItem as DCU_ROUTER;
                if (dCU != null)
                {

                    var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TOA_DO?matram=" + viewModel.SelectItemTram.MA_TRAM + "&SerialID=" + dCU.DcuID + "&ma_Cot=" + dCU.MA_COT).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        ObservableCollection<GoogleMap> toado = JsonConvert.DeserializeObject<ObservableCollection<GoogleMap>>(result);

                        if (toado != null)
                        {
                            await Xamarin.Essentials.Map.OpenAsync(toado[0].Lat, toado[0].Lng, new MapLaunchOptions { NavigationMode = Xamarin.Essentials.NavigationMode.Driving });
                        }
                    }
                    else
                    {
                        await new MessageBox("Thông Báo", "Không tìm thấy tọa độ cho vị trí này").Show();
                    }
                }

            }
            catch (Exception ex)
            {
                await new MessageBox("Lỗi", ex.Message).Show();
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

        private async  void btnDongBo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.SelectItemTram == null )
                {
                    await new MessageBox("thông báo", "Vui lòng chọn trạm").Show();
                    return;
                }    
                DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(viewModel.SelectItemTram.ID_DCU ), MaDviQly = viewModel.SelectItemDonVi.MA_DON_VI , MaTram = viewModel.SelectItemTram.MA_TRAM, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = "", Path = "", Type = "", TypeReq = "Sys" };
                MqttClientRepository.PublibMessage("EMEC/RFSPIDER/APP", JsonConvert.SerializeObject(dcuMqtt));
               // DependencyService.Get<IMessage>().ShortAlert("Đã gửi yêu cầu đồng bộ. vui lòng đợi...");
            }
            catch (Exception)
            {

                
            }
           
        }

        private async void btnTimKhachHang_Clicked(object sender, EventArgs e)
        {
            if (viewModel.SelectItemDonVi == null)
            {
                await new MessageBox("thông báo", "chọn điện lực cần kiểm tra").Show();
                return;
            }    
            await Navigation.PushAsync(new Tim_Kiem_Khach_Hang (viewModel.SelectItemDonVi.MA_DON_VI, viewModel.SelectItemDonVi.TEN_DON_VI));
        }

        private void cbDienLuc_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbTram.SelectedIndex != -1 )
                {
                    int index = cbTram.SelectedIndex;
                    TRAM tr = viewModel.Trams[index] as TRAM;
                }    
            }
            catch (Exception ex)
            {

              
            }
        }

        private async void btnDocTram_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.SelectItemTram == null)
                {
                    await new MessageBox("thông báo", "Vui lòng chọn trạm").Show();
                    return;
                }
                DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(viewModel.SelectItemTram.ID_DCU), MaDviQly = viewModel.SelectItemDonVi.MA_DON_VI, MaTram = viewModel.SelectItemTram.MA_TRAM, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = "", Path = "", Type = "", TypeReq = "ReadTram" };
                MqttClientRepository.PublibMessage("EMEC/RFSPIDER/APP", JsonConvert.SerializeObject(dcuMqtt));
                DependencyService.Get<IMessage>().ShortAlert("Đã gửi yêu cầu đọc toàn trạm. vui lòng chờ kết quả trả về");
            }
            catch (Exception)
            {


            }
        }
    }
    public class Dark : DataGridStyle
    {
        public Dark()
        {
        }

        public override Color GetHeaderBackgroundColor()
        {
            return Color.FromHex("#149C62");
        }

        public override Color GetHeaderForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }
        public override Color GetRowDragViewIndicatorColor()
        {
            return Color.SkyBlue;
        }
        //public override Color GetRecordBackgroundColor()
        //{
        //    return Color.FromRgb(43, 43, 43);
        //}

        public override Color GetRecordForegroundColor()
        {
            return Color.Black;
        }

        public override Color GetSelectionBackgroundColor()
        {
            return Color.OrangeRed;
        }

        public override Color GetSelectionForegroundColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public override Color GetCaptionSummaryRowBackgroundColor()
        {
            return Color.SkyBlue;
        }
        public override Color GetCaptionSummaryRowForeGroundColor()
        {
            return Color.White;
        }


        public override Color GetBorderColor()
        {
            return Color.FromRgb(81, 83, 82);
        }

        public override Color GetLoadMoreViewBackgroundColor()
        {
            return Color.FromRgb(242, 242, 242);
        }

        //public override Color GetLoadMoreViewForegroundColor()
        //{
        //    return Color.FromRgb(34, 31, 31);
        //}

        //public override Color GetAlternatingRowBackgroundColor()
        //{
        //    return Color.Yellow;
        //}
    }
}
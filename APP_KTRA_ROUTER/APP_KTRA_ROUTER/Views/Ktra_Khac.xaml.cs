using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Views;
using APP_KTRA_ROUTER.ViewModels;
using APP_KTRA_ROUTER.Popup;
using APP_KTRA_ROUTER.Global;
using Xamarin.Essentials;
using APP_KTRA_ROUTER.Interface;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Globalization;
using ZXing.Net.Mobile.Forms;
using APP_KTRA_ROUTER.Services;
using Newtonsoft.Json.Bson.Converters;

namespace APP_KTRA_ROUTER.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class Ktra_Khac : ContentPage, INotifyPropertyChanged
    {
        Save_PathReposity savepath { get; set; }
        KiemTraViewModel viewModel;
        ObservableCollection<DcuMqttResp> lstResult = new ObservableCollection<DcuMqttResp>();
        public Ktra_Khac()
        {
            InitializeComponent();
            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
            {
                Navigation.PushAsync(new Setting());
            }
            savepath = new Save_PathReposity(App.gCS_Dbcontext);
            BindingContext = viewModel = new KiemTraViewModel();
            Config.messagingCenter.Subscribe<SubscribeCallback, DcuMqttResp>(this, "MQTT", (obj, item) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    try
                    {
                        if (item.TenDangNhap != Preferences.Get(Config.User, "")) return;

                        if (item.MeterID == idCongTo.Text.Trim())
                        {
                            lblThongBao.Text = "";
                            if (lstResult.Count == 1)
                            {
                                if (lstResult[0].ErrorCode .Contains("Đã nhận được yêu cầu"))
                                lstResult.Clear();
                            }
                            if (item.ErrorCode != "")
                            {
                                item.RowHeight = 0;
                            }    
                            else
                            {
                               
                                item.RowHeight = GridLength.Auto;
                            }    
                            lstResult.Add(item);
                            lstViewResult.ItemsSource = "";
                            lstViewResult.ItemsSource = lstResult;
                            lstViewResult.ScrollTo(item, ScrollToPosition.Center, true);
                        }
                       
                        if (item.TypeReq == null) return;
                        if (item.MaTram != viewModel.SelectItemTram.MA_TRAM) return;
                        if ( item.TypeReq.ToLower() == "readtram")
                        {
                            DependencyService.Get<IMessage>().ShortAlert(item.ErrorCode);
                        }
                    }
                    catch 
                    {                        
                    }
                    


                });
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;         
            if (viewModel.DonVis.Count == 0)
            {
                viewModel.LoadDonvisCommand.Execute(null);
                viewModel.LoadChungLoaisCommand.Execute(null);
            }      
        }
        
        private async void check_Clicked(object sender, EventArgs e)            
        {
            //kiểm tra quyền xem có đc can thiệp hes đọc công tơ không
            string str = Config.URL + "api/home/getQuyenDoc?username=" + Xamarin.Essentials.Preferences.Get(Config.User, "");
            var _json = Config.client.GetStringAsync(str).Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
            {
            }
            else
            {
                await new MessageBox("Thông Báo", "Bạn không có quyền thao tác chức năng này. vui lòng liên hệ IT CPC_EMEC để cấp quyền").Show();
                return;
            }    

                try
               {
                if (viewModel.SelectItemDonVi == null)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn điện lực");
                    return;
                }
                if (viewModel.SelectItemTram == null)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn trạm");
                    return;
                }

               // lblThongBao.Text = "Đã gửi yêu cầu. đang chờ phản hồi";
                lstResult.Clear();
                //string path = cbPath.Text.Split(':')[0];
                //if (path != "") path = path + ";" + idCongTo.Text.Trim();
                DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(viewModel.SelectItemTram.ID_DCU), MaDviQly = viewModel.SelectItemDonVi.MA_DON_VI, MaTram = viewModel.SelectItemTram.MA_TRAM, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = idCongTo.Text.Trim(), Path = cbPath.Text, Type = cbchungloai.Text, TypeReq= "Disco" };
                MqttClientRepository.PublibMessage("EMEC/RFSPIDER/APP", JsonConvert.SerializeObject(dcuMqtt));
                DependencyService.Get<IMessage>().ShortAlert("Đã gửi yêu cầu. đang chờ phản hồi");
            }
            catch (Exception ex)
            {

               await  new MessageBox("Thông báo", ex.Message).Show();
            }
                   
        }

        private async void scan_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            scan.Title = "Quét barcode công tơ";
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    idCongTo.Text = result.Text;
                    viewModel.TimCongToCommand.Execute(result.Text);
                });

            };
        }

        protected   override bool OnBackButtonPressed()
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

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            viewModel.LoadDonvisCommand.Execute(null);
            if (viewModel.DonVis.Count > 0)
            {
                foreach (DonVi dv in viewModel.DonVis)
                {
                    viewModel.SelectItemDonVi = dv;
                    break;
                }    
            }    
            
        }

        private async void btnSavePath_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (viewModel.SelectItemTram == null)
                {
                    await new MessageBox("thông báo", "Vui lòng chọn trạm").Show();
                    return;
                }
                if (cbPath.Text == "")
                {
                    await new MessageBox("thông báo", "Vui lòng nhập đường dẫn").Show();
                    return;
                }
                var ok = await new MessageYESNO("thông báo", "bạn có muốn lưu đường dẫn này không").Show();
                if (ok == DialogReturn.OK)
                {
                    Database.Save_Path path = new Database.Save_Path();
                    path.Path = cbPath.Text;
                    path.MA_TRAM = viewModel.SelectItemTram.MA_TRAM;
                    await savepath.AddItemAsync(path);
                    DependencyService.Get<IMessage>().ShortAlert("Đã lưu thành công");//   await new MessageBox("thông báo", "Đã lưu thành công").Show();
                    viewModel.LoadPathsCommand.Execute(viewModel.SelectItemTram);
                }
            }
            catch (Exception ex)
            {


            }
               
               
        }

        private async void check_Clicked_1(object sender, EventArgs e)
        {
            //should use string.IsNullOrEmpty
            if (idCongTo.Text == "" || idCongTo.Text == null)
            {
                await new MessageBox("Thông báo", "nhập số công tơ/router để kiểm tra").Show();
                return;
            }         
            viewModel.TimCongToCommand.Execute(idCongTo.Text);          
        }

        private async void stop_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.SelectItemDonVi == null)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn điện lực");
                    return;
                }
                if (viewModel.SelectItemTram == null)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Vui lòng chọn trạm");
                    return;
                }             
                string path = cbPath.Text.Split(':')[0];
                if (path != "") path = path + ";" + idCongTo.Text.Trim();
                DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(viewModel.SelectItemTram.ID_DCU), MaDviQly = viewModel.SelectItemDonVi.MA_DON_VI, MaTram = viewModel.SelectItemTram.MA_TRAM, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = idCongTo.Text.Trim(), Path = path, Type = cbchungloai.Text, TypeReq = "Stop" };
                MqttClientRepository.PublibMessage("EMEC/RFSPIDER/APP", JsonConvert.SerializeObject(dcuMqtt));
                DependencyService.Get<IMessage>().ShortAlert("Đã gửi yêu cầu ngưng đọc");
            }
            catch (Exception ex)
            {

                await new MessageBox("Thông báo", ex.Message).Show();
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
   
    
}
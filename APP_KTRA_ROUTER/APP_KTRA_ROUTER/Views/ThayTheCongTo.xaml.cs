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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThayTheCongTo : ContentPage
    {
        public string URL_API = "https://smart.cpc.vn/DSPM_Api/";
        public ThayTheCongTo()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");
            var _json = Config.client.GetStringAsync(URL_API + "api/modem/getPhanQuyen?user=" + Preferences.Get(Config.User, "")).Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                var response = JsonConvert.DeserializeObject<ObservableCollection<USER_INFO>>(result);
                if (response[0].USER_NAME == "")
                {
                    await DependencyService.Get<IProcessLoader>().Hide();
                    await new MessageBox("Thông Báo", "Anh/ chị không được quyền truy cập tính năng này. Vui lòng liên hệ CPCEMEC để biết chi tiết.").Show();
                    await Shell.Current.Navigation.PushAsync(new Home());
                }
                else
                {
                    await DependencyService.Get<IProcessLoader>().Hide();
                }
            }
            else
            {
                await DependencyService.Get<IProcessLoader>().Hide();
                await new MessageBox("Thông Báo", "Anh/ chị không được quyền truy cập tính năng này. Vui lòng liên hệ CPCEMEC để biết chi tiết.").Show();
                await Shell.Current.Navigation.PushAsync(new Home());
            }
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

        private async void scanIMEIBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                });

            };
        }

        private async void scanIMEIBtnMoi_Clicked(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    IMEITextMoi.Text = result.Text.PadLeft(12, '0');
                });

            };
        }

        private async void btnThayThe_Clicked(object sender, EventArgs e)
        {
            if (IMEITextCu.Text == null || IMEITextCu.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét serial công tơ cũ").Show();
                return;
            }
            if (IMEITextMoi.Text == null || IMEITextMoi.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét serial công tơ mới").Show();
                return;
            }
            IMEITextMoi.Text = IMEITextMoi.Text.PadLeft(12, '0');
            if (IMEITextMoi.Text == IMEITextCu.Text)
            {
                await new MessageBox("thông báo", "Số serial mới không được trùng với số serial cũ").Show();
                return;
            }
            //var ok = await new MessageXacThucMatKhau().Show();
            //if (ok == Global.DialogReturn.OK)
            {
                await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");
                var _json = Config.client.GetStringAsync(URL_API + "api/modem/getInfoBySerial?serial=" + IMEITextCu.Text).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if ( _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    var response = JsonConvert.DeserializeObject<ObservableCollection<INFO_CONGTO>>(result);
                    if (response[0].ASSETID == "")
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        await new MessageBox("Thông Báo", "Không tìm thấy thông tin mã điểm đo của số serial cũ").Show();
                    }
                    else
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        var ok1 = await new MessageYESNO("Thông báo", "Anh chị có chắn chắn muốn thay serial " + IMEITextCu.Text + " của điểm đo " + response[0].ASSETID + " bằng serial " + IMEITextMoi.Text + " không?").Show();
                        if (ok1 == DialogReturn.OK)
                        {
                            await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");
                            var _json1 = Config.client.PostAsync(URL_API + "api/modem/updateSerial?serial_cu=" + IMEITextCu.Text + "&serial_moi=" + IMEITextMoi.Text + "&ma_ddo=" + response[0].ASSETID, null).Result;
                            var content = _json1.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();
                            if (content == "1")
                            {
                                HISTORY req = new HISTORY();
                                req.MaDDo = response[0].ASSETID;
                                req.IMEICu = IMEITextCu.Text;
                                req.IMEIMoi = IMEITextMoi.Text;
                                req.NguoiSua = Preferences.Get(Config.User, "");
                                req.ChuyenMD = false;
                                req.LoaiThay = 2;
                                HttpContent httpcontent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                                var _json2 = Config.client.PostAsync(URL_API + "api/modem/insertHistory", httpcontent).Result;
                                //var content2 = _json2.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();
                                //if (content == "1")
                                {
                                    await DependencyService.Get<IProcessLoader>().Hide();
                                    await new MessageBox("Thông Báo", "Thay thế serial thành công").Show();
                                }    
                            }
                            else
                            {
                                await DependencyService.Get<IProcessLoader>().Hide();
                                await new MessageBox("Thông Báo", "Có lỗi trong quá trình thay thế").Show();
                            }    
                        }
                    }    
                }
                else
                {
                    await DependencyService.Get<IProcessLoader>().Hide();
                    await new MessageBox("Thông Báo", "Không tìm thấy thông tin serial cũ").Show();
                }
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {  
                Shell.Current.Navigation.PushAsync(new LichSuModem(2));
            });
        }
    }
}
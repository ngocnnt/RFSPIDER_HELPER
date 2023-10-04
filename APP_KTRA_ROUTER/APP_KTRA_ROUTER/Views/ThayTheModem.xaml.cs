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
    public partial class ThayTheModem : ContentPage
    {
        public string URL_API = "https://smart.cpc.vn/DSPM_Api/";
        public ThayTheModem()
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
                    if (result.Text.Length == 16) IMEITextCu.Text = result.Text.Substring(0, 15);
                    else IMEITextCu.Text = result.Text;
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
                    if (result.Text.Length == 16) IMEITextMoi.Text = result.Text.Substring(0, 15);
                    else IMEITextMoi.Text = result.Text;
                });

            };
        }

        private async void btnThayThe_Clicked(object sender, EventArgs e)
        {
            if (IMEITextCu.Text == null || IMEITextCu.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã IMEI cũ").Show();
                return;
            }
            if (IMEITextMoi.Text == null || IMEITextMoi.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã IMEI mới").Show();
                return;
            }
            if (IMEITextCu.Text.Length != 15)
            {
                await new MessageBox("thông báo", "Số IMEI cũ không đúng định dạng").Show();
                return;
            }
            if (IMEITextMoi.Text.Length != 15)
            {
                await new MessageBox("thông báo", "Số IMEI mới không đúng định dạng").Show();
                return;
            }
            if (IMEITextMoi.Text == IMEITextCu.Text)
            {
                await new MessageBox("thông báo", "Số IMEI mới không được trùng với số IMEI cũ").Show();
                return;
            }
            //var ok = await new MessageXacThucMatKhau().Show();
            //if (ok == Global.DialogReturn.OK)
            {
                await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");

                var _json3 = Config.client.GetStringAsync(URL_API + "api/modem/getInfoByImei?imei=" + IMEITextMoi.Text).Result;
                _json3 = _json3.Replace("\\r\\n", "").Replace("\\", "");
                if (_json3.Contains("[]") == false)
                {
                    Int32 from3 = _json3.IndexOf("[");
                    Int32 to3 = _json3.IndexOf("]");
                    string result3 = _json3.Substring(from3, to3 - from3 + 1);
                    var response3 = JsonConvert.DeserializeObject<ObservableCollection<INFO_MODEM>>(result3);
                    await DependencyService.Get<IProcessLoader>().Hide();
                    await new MessageBox("Thông Báo", "IMEI " + IMEITextMoi.Text + " đã được khai báo cho mã điểm đo " + response3[0].OBJID + ". Anh/ chị vui lòng kiểm tra lại!").Show();
                    return;
                }

                var _json = Config.client.GetStringAsync(URL_API + "api/modem/getInfoByImei?imei=" + IMEITextCu.Text).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if ( _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    var response = JsonConvert.DeserializeObject<ObservableCollection<INFO_MODEM>>(result);
                    if (response[0].OBJID == "")
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        await new MessageBox("Thông Báo", "Không tìm thấy thông tin mã điểm đo của IMEI cũ").Show();
                    }
                    else
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        var ok1 = await new MessageYESNO("Thông báo", "Anh chị có chắn chắn muốn thay IMEI " + IMEITextCu.Text + " của điểm đo " + response[0].OBJID + " bằng IMEI " + IMEITextMoi.Text + ((cbMayDoc.IsChecked == true) ? " và chuyển sang Máy đọc modem 4G của EMEC" : "") + " không?").Show();
                        if (ok1 == DialogReturn.OK)
                        {
                            await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");
                            var _json1 = Config.client.PostAsync(URL_API + "api/modem/updateImei?imei_cu=" + IMEITextCu.Text + "&imei_moi=" + IMEITextMoi.Text + "&ma_ddo=" + response[0].OBJID + "&may_doc=" + (bool)cbMayDoc.IsChecked, null).Result;
                            var content = _json1.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();
                            if (content == "1")
                            {
                                HISTORY req = new HISTORY();
                                req.MaDDo = response[0].OBJID;
                                req.IMEICu = IMEITextCu.Text;
                                req.IMEIMoi = IMEITextMoi.Text;
                                req.NguoiSua = Preferences.Get(Config.User, "");
                                req.ChuyenMD = (bool)cbMayDoc.IsChecked;
                                req.LoaiThay = 1;
                                HttpContent httpcontent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                                var _json2 = Config.client.PostAsync(URL_API + "api/modem/insertHistory", httpcontent).Result;
                                //var content2 = _json2.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();
                                //if (content == "1")
                                {
                                    await DependencyService.Get<IProcessLoader>().Hide();
                                    await new MessageBox("Thông Báo", "Thay thế IMEI thành công").Show();
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
                    await new MessageBox("Thông Báo", "Không tìm thấy thông tin IMEI cũ").Show();
                }
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {  
                Shell.Current.Navigation.PushAsync(new LichSuModem(1));
            });
        }
    }
}
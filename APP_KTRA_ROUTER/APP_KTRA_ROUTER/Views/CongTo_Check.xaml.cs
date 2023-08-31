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

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CongTo_Check : ContentPage
    {
       public List<DcuMqttResp> lstResult = new List<DcuMqttResp>();
        DCU_ROUTER DCU_ { get; set; }
        PATH path { get; set; }
        string _madvql,_matram, _madonvi;
        MqttClientRepository repository = new MqttClientRepository();
        public CongTo_Check( string matram, string madonvi, DCU_ROUTER dcu)
        {
            InitializeComponent();
            DCU_ = dcu;
            _madonvi = madonvi;
            _matram = matram;
            Title ="Công tơ: "+  dcu.MeterID;

            _madvql = madonvi.Substring(0, 2) == "PC" ? madonvi.Substring(0, 4) : madonvi.Substring(0, 2);
            string topic = "RESPOND/CPC/" + _madvql + "/" + madonvi + "/" + matram;
            MqttClientRepository.client = repository.Create("222.255.138.213", 1883, "lucnv", "lucnv", new List<string> { topic }, Guid.NewGuid().ToString());//

            MessagingCenter.Subscribe<SubscribeCallback, DcuMqttResp>(this, "MQTT", (obj, item) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    try
                    {
                        if (item.TenDangNhap != Preferences.Get(Config.User, "")) return;
                        if (item.MeterID == DCU_.MeterID)
                        {
                            lblResult.Text = "";
                            Title = "Công tơ: " + DCU_.MeterID;
                            lstResult.Clear();
                            if (item.NgayGio != null)
                            {
                                item.RowHeight = GridLength.Auto;
                            }
                            else
                            {
                                item.RowHeight = 0;
                            }
                            lstResult.Add(item);
                            lstViewResult.ItemsSource = "";
                            lstViewResult.ItemsSource = lstResult;
                            // lstViewResult.ScrollTo(lstResult[lstResult.Count - 1], ScrollToPosition.End, true);
                        }
                    }
                    catch               
                    {                        
                    }
                });
            });
        }

        

        private  async void Send_Clicked(object sender, EventArgs e)
        {
            try
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
                Title = "Checking: " + DCU_.MeterID;

                string typereq = "Reg";
                switch (cbChungLoai.SelectedIndex)
                {
                    case 0:
                        typereq = "Reg";
                        break;
                    case 1:
                        typereq = "Ins";
                        break;
                }
                if (cbPath.Text == null)
                {

                    DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(DCU_.DcuID), MaDviQly = _madonvi, MaTram = _matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = DCU_.MeterID, Path = "", Type = DCU_.Type, TypeReq = typereq, Time = DateTime.Now.ToString("yyyyMMdd HHmmss") };
                    MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC, "").Replace("MA_DVIQLY", _madvql), JsonConvert.SerializeObject(dcuMqtt));
                    lblResult.Text = "Đã gửi bản tin yêu cầu. đang chờ phản hồi" + Environment.NewLine;
                    Send.IsEnabled = false;
                    Send.BackgroundColor = Color.Gray;
                }
                else if (cbPath.Text == "")
                {
                    try
                    {
                        DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(DCU_.DcuID), MaDviQly = _madonvi, MaTram = _matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = DCU_.MeterID, Path = cbPath.SelectedValue.ToString(), Type = DCU_.Type, TypeReq = typereq, Time = DateTime.Now.ToString("yyyyMMdd HHmmss") };
                        MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC, "").Replace("MA_DVIQLY", _madvql), JsonConvert.SerializeObject(dcuMqtt));
                        lblResult.Text = "Đã gửi bản tin yêu cầu. đang chờ phản hồi" + Environment.NewLine;
                        Send.IsEnabled = false;
                        Send.BackgroundColor = Color.Gray;
                    }
                    catch (Exception)
                    {

                        DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(DCU_.DcuID), MaDviQly = _madonvi, MaTram = _matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = DCU_.MeterID, Path = "", Type = DCU_.Type, TypeReq = typereq, Time = DateTime.Now.ToString("yyyyMMdd HHmmss") };
                        MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC, "").Replace("MA_DVIQLY", _madvql), JsonConvert.SerializeObject(dcuMqtt));
                        lblResult.Text = "Đã gửi bản tin yêu cầu. đang chờ phản hồi" + Environment.NewLine;
                        Send.IsEnabled = false;
                        Send.BackgroundColor = Color.Gray;
                    }

                }
                else

                {
                    DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(DCU_.DcuID), MaDviQly = _madonvi, MaTram = _matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = DCU_.MeterID, Path = cbPath.Text, Type = DCU_.Type, TypeReq = typereq, Time = DateTime.Now.ToString("yyyyMMdd HHmmss") };
                    MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC, "").Replace("MA_DVIQLY", _madvql), JsonConvert.SerializeObject(dcuMqtt));
                    lblResult.Text = "Đã gửi bản tin yêu cầu. đang chờ phản hồi" + Environment.NewLine;
                    Send.IsEnabled = false;
                    Send.BackgroundColor = Color.Gray;
                }
            }
            catch (Exception)
            {
            }
            finally {
                await Task.Delay(10000);
                Send.IsEnabled = true;
                Send.BackgroundColor = Color.Green;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/get_path?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&serial=" + DCU_.MeterID).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ObservableCollection<PATH> listpath = JsonConvert.DeserializeObject<ObservableCollection<PATH>>(result);
                    cbPath.DataSource = listpath;
                    if (listpath.Count > 0)
                    {
                        cbPath.SelectedIndex = 0;
                    }
                }


            }
            catch (Exception)
            {

              
            }
           
            
        }
    }
}
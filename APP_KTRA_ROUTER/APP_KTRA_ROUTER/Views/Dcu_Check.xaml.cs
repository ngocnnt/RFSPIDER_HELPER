using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dcu_Check : ContentPage
    {
        DCU_ROUTER dCU;
        string _madvql,_matram, _madonvi;
        MqttClientRepository repository = new MqttClientRepository();
        public Dcu_Check(string DCU_ID ,string matram,string madonvi, DCU_ROUTER dcu)
        {
            InitializeComponent();
            Title = "Checking " + DCU_ID + "....";
            dCU = dcu;
            _madonvi = madonvi;
            _matram = matram;

            _madvql = madonvi.Substring(0, 2) == "PC" ? madonvi.Substring(0, 4) : madonvi.Substring(0, 2);
            string topic = "RESPOND/CPC/" + _madvql + "/" + madonvi + "/" + matram;
            MqttClientRepository.client = repository.Create("222.255.138.213", 1883, "lucnv", "lucnv", new List<string> { topic }, Guid.NewGuid().ToString());//

            //yêu cầu server kiểm tra DCU này
            DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(dcu.DcuID), MaDviQly = madonvi, MaTram = matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = dcu.MeterID, Path = dcu.Path , Type = dcu.Type,TypeReq="Reg", Time = DateTime.Now.ToString("yyyyMMdd HHmmss") };
            MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC,"").Replace("MA_DVIQLY", _madvql), JsonConvert.SerializeObject(dcuMqtt));
           
            MessagingCenter.Subscribe<SubscribeCallback, DcuMqttResp>(this, "MQTT", (obj, item) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    if (item.DcuID ==Convert.ToUInt32 ( dCU.DcuID) && item.Type =="DCU")
                    {
                        lblTrangThai.Text =  item.TrangThai;
                        if (item.TrangThai.ToLower() == "online")
                        {
                            lblTrangThai.TextColor = Color.Green;
                            lblTrangThai.Text.ToUpper();
                        }                   
                        else
                        {
                            lblTrangThai.TextColor = Color.Red;
                            lblTrangThai.Text.ToUpper();
                        }
                        try
                        {
                            lblchisosong.Text = item.CSQ.Replace("+CSQ:", "").Trim();
                           double.TryParse(lblchisosong.Text, out double  chiso);
                            if (chiso < 20 )
                            {
                                lblchisosong.TextColor = Color.Yellow;
                            }    
                            else if ( chiso >=20 && chiso < 30)
                            {
                                lblchisosong.TextColor = Color.Blue ;
                            }    
                            else
                            {
                                lblchisosong.TextColor = Color.Green ;
                            }
                            
                        }
                        catch (Exception)
                        {

                            
                        }
                        lblngaygio.Text = item.NgayGio.ToString ();
                        lblmaloi.Text = item.ErrorCode;                          
                       
                    }    
                    
                });
            });


        }

        private void ResetMQTT_Clicked(object sender, EventArgs e)
        {

        }

        private void Send_Clicked(object sender, EventArgs e)
        {
            string madvql = _madonvi.Substring(0, 2) == "PC" ? _madonvi.Substring(0, 4) : _madonvi.Substring(0, 2);
            DcuMqttReq dcuMqtt = new DcuMqttReq { DcuID = Convert.ToUInt32(dCU.DcuID), MaDviQly = _madonvi, MaTram = _matram, TenDangNhap = Preferences.Get(Config.User, ""), MeterID = dCU.MeterID, Path = dCU.Path, Type = dCU.Type , TypeReq="Reg" , Time = DateTime.Now.ToString("yyyyMMdd HHmmss")};
            MqttClientRepository.PublibMessage(Preferences.Get(Config.TOPIC, "").Replace("MA_DVIQLY", madvql), JsonConvert.SerializeObject(dcuMqtt));
            DependencyService.Get<IMessage>().ShortAlert("Đã gửi lại bản tin. vui lòng chờ...");
        }
    }
}
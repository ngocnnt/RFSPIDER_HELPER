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

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KiemTraModem : ContentPage
    {
        public string brokerHost = "10.72.96.111";
        public int brokerPort = 9291;
        public string brokerUsername = "evnhes";
        public string brokerPassword = "evnhes";
        public string baseTopic2 = "HES3G/CHECKSTATUS/IMEI/";
        public string baseTopic1 = "HES3G/RECEIVED/IMEI/";
        public string baseTopicData = "HES3G/READ/IMEI/";
        public string topic1 = "";
        public string topic2 = "";
        public string topicData = "";
        public DateTime tungay = new DateTime();
        public DateTime denngay = new DateTime();
        MqttClient MqClient;
        public class MqttRE
        {
            public string active { get; set; }
            public string status { get; set; }
            public string timeDisconnect { get; set; }
            public string timeConnect { get; set; }
            public string IpModem { get; set; }
            public string csq { get; set; }
            public string countConnect { get; set; }
            public string ipServer { get; set; }
        }

        public KiemTraModem()

        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                    if (result.Text.Length == 16) IMEIText.Text = result.Text.Substring(0, 15);
                    else IMEIText.Text = result.Text;
                });

            };
        }

        private async void btnKiemTra_Clicked(object sender, EventArgs e)
        {
            mqttEntry.Text = "";
            if (IMEIText.Text == null || IMEIText.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã IMEI").Show();
                return;
            }
            if (IMEIText.Text.Length != 15)
            {
                await new MessageBox("thông báo", "Số IMEI không đúng định dạng").Show();
                return;
            }
            string message = "";
            string imei = IMEIText.Text;
            try
            {
                MqClient = new MqttClient(brokerHost, brokerPort, false, null, null, MqttSslProtocols.None);
                MqClient.Connect("", brokerUsername, brokerPassword);
                if (MqClient.IsConnected)
                {
                    string[] topics = new string[1];
                    topic1 = baseTopic1 + imei;
                    topics[0] = topic1;
                    byte[] msgs = new byte[1];
                    msgs[0] = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
                    MqClient.Subscribe(topics, msgs);
                    MqClient.MqttMsgPublishReceived += MqClient_MqttMsgPublishReceived;
                }

                if (!string.IsNullOrEmpty(imei))
                {
                    // Concatenate the IMEI to the base topic
                    topic2 = baseTopic2 + imei;
                    // Now you can use the 'topic2' to publish your MQTT message
                    MqClient.Publish(topic2, Encoding.UTF8.GetBytes(message));
                }
            }

            catch (Exception ex)
            {
                await new MessageBox("thông báo", "Lỗi dữ liệu, anh/ chị vui lòng kiểm tra lại!").Show();
            }
        }
        private async void MqClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            string value = Encoding.UTF8.GetString(e.Message);
            if (value.Length <= 5) return;
            if (value.ToLower().Contains("not active")) return;
            if (value.Contains(IMEIText.Text)) return;
            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    var root = JsonConvert.DeserializeObject<MqttRE>(value);
                    if (root.active.ToString().ToUpper() == "ACTIVE")
                    {
                        mqttEntry.Text = "";
                        mqttEntry.Text += "Trạng thái khai báo: " + root.active.ToString() + Environment.NewLine;
                        mqttEntry.Text += "Trạng thái Modem: " + root.status.ToString() + Environment.NewLine;
                        //mqttEntry.Text += "timeDisconnect: " + root.timeDisconnect.ToString() + Environment.NewLine;
                        mqttEntry.Text += "Thời gian kết nối gần nhất: " + root.timeConnect.ToString() + Environment.NewLine;
                        mqttEntry.Text += "Ip Modem: " + root.IpModem.ToString();
                        if (root.IpModem != null && root.IpModem.ToString() != "")
                            switch (root.IpModem.ToString().Split('.')[0])
                            {
                                case "113":
                                case "116":
                                case "117":
                                    mqttEntry.Text += " (Vinaphone)" + Environment.NewLine;
                                    break;
                                case "59":
                                case "42":
                                    mqttEntry.Text += " (Mobifone)" + Environment.NewLine;
                                    break;
                                case "27":
                                case "171":
                                    mqttEntry.Text += " (Viettel)" + Environment.NewLine;
                                    break;
                            }
                        else mqttEntry.Text += Environment.NewLine;
                        mqttEntry.Text += "CSQ: " + root.csq.ToString() + Environment.NewLine;
                        mqttEntry.Text += "Số lần mất kết nối: " + root.countConnect.ToString() + Environment.NewLine;
                        mqttEntry.Text += "Ip Server: " + root.ipServer.ToString() + Environment.NewLine;
                    }
                    else
                    {
                        if (!mqttEntry.Text.ToLower().Contains("trạng thái khai báo: active"))
                        {
                            mqttEntry.Text = "Vui lòng kiểm tra lại IMEI hoặc khai báo IMEI trên EVNHES";
                        }
                    }
                });

            }

            catch (Exception ex)
            {
                //await new MessageBox("thông báo", "Lỗi dữ liệu, anh/ chị vui lòng kiểm tra lại!").Show();
            }
        }

        private async void btnRead_ClickedAsync(object sender, EventArgs e)
        {
            mqttEntry.Text = "";
            if (IMEIText.Text == null || IMEIText.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng nhập hoặc quét mã IMEI").Show();
                return;
            }
            if (IMEIText.Text.Length != 15)
            {
                await new MessageBox("thông báo", "Số IMEI không đúng định dạng").Show();
                return;
            }
            if (cbLoaiData.Text == null || cbLoaiData.Text == "")
            {
                await new MessageBox("thông báo", "Vui lòng chọn loại đọc").Show();
                return;
            }
            if (tungay == new DateTime())
            {
                await new MessageBox("thông báo", "Vui lòng chọn từ ngày").Show();
                return;
            }
            if (denngay == new DateTime())
            {
                await new MessageBox("thông báo", "Vui lòng chọn đến ngày").Show();
                return;
            }
            //"{'type':'CurrentValue','fromdate':'2023-07-28','todate':'2023-07-28'}"
            string message = "{'type':";
            switch (cbLoaiData.SelectedValue)
            {
                case "Chỉ số tức thời":
                    message += "'CurrentValue','fromdate':";
                    break;
                case "Thông số vận hành":
                    message += "'Instantaneous','fromdate':";
                    break;
                case "Loadprofile":
                    message += "'LoadProfile','fromdate':";
                    break;
                case "Chỉ số chốt":
                    message += "'History','fromdate':";
                    break;
                case "Công suất cực đại":
                    message += "'MaxDemand','fromdate':";
                    break;
                case "Sự kiện":
                    message += "'Event','fromdate':";
                    break;
            }
            message += "'" + tungay.ToString("yyyy-MM-dd") + "','todate':'" + denngay.ToString("yyyy-MM-dd") + "'}";
            string imei = IMEIText.Text;
            try
            {
                MqClient = new MqttClient(brokerHost, brokerPort, false, null, null, MqttSslProtocols.None);
                MqClient.Connect("", brokerUsername, brokerPassword);
                if (MqClient.IsConnected)
                {
                    string[] topics = new string[1];
                    topic1 = baseTopic1 + imei;
                    topics[0] = topic1;
                    byte[] msgs = new byte[1];
                    msgs[0] = MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE;
                    MqClient.Subscribe(topics, msgs);
                    MqClient.MqttMsgPublishReceived += MqClient_MqttMsgPublishReceivedData;
                }

                if (!string.IsNullOrEmpty(imei))
                {
                    // Concatenate the IMEI to the base topic
                    topicData = baseTopicData + imei;
                    // Now you can use the 'topic2' to publish your MQTT message
                    MqClient.Publish(topicData, Encoding.UTF8.GetBytes(message));
                }
            }

            catch (Exception ex)
            {
                await new MessageBox("thông báo", "Lỗi dữ liệu, anh/ chị vui lòng kiểm tra lại!").Show();
            }
        }

        private async void MqClient_MqttMsgPublishReceivedData(object sender, MqttMsgPublishEventArgs e)
        {
            string topic = e.Topic;
            string value = Encoding.UTF8.GetString(e.Message);
            if (value.ToLower().Contains("status")) return;
            try
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    if (value.Contains(IMEIText.Text))
                    {

                        if (value.ToUpper().Contains("BUSY"))
                        {
                            mqttEntry.Text = IMEIText.Text + " - Modem đang bận đọc theo tiến trình." + Environment.NewLine;
                        }
                        if (value.ToUpper().Contains("READ"))
                            mqttEntry.Text = IMEIText.Text + " - Đang thực hiện đọc...Vui lòng đợi..." + Environment.NewLine;
                        if (value.ToUpper().Contains("COMPLETE"))
                        {
                            if (mqttEntry.Text.Contains(IMEIText.Text + " - Đã hoàn thành đọc."))
                                return;
                            mqttEntry.Text += IMEIText.Text + " - Đã hoàn thành đọc." + Environment.NewLine;
                        }
                    }
                    else
                    {
                        if (!mqttEntry.Text.Contains(IMEIText.Text))
                        {
                            mqttEntry.Text = "Vui lòng kiểm tra lại IMEI hoặc khai báo IMEI trên EVNHES";
                        }
                    }
                });
            }

            catch (Exception ex)
            {
                //await new MessageBox("thông báo", ex.Message).Show();
            }
        }

        private void Datepicker_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
        {
            tungay = (DateTime)e.NewValue;
            pickerTuNgay.Text = tungay.ToString("dd/MM/yyyy");
        }

        private void TuNgay_Clicked(object sender, EventArgs e)
        {
            SelectTuNgay.IsOpen = true;
            tungay = SelectTuNgay.Date;
            pickerTuNgay.Text = tungay.ToString("dd/MM/yyyy");
        }

        private void SelectDenNgay_DateSelected(object sender, Syncfusion.XForms.Pickers.DateChangedEventArgs e)
        {
            denngay = (DateTime)e.NewValue;
            pickerDenNgay.Text = denngay.ToString("dd/MM/yyyy"); ;
        }

        private void DenNgay_Clicked(object sender, EventArgs e)
        {
            SelectDenNgay.IsOpen = true;
            denngay = SelectDenNgay.Date;
            pickerDenNgay.Text = denngay.ToString("dd/MM/yyyy"); ;
        }

        private void cbLoaiData_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (cbLoaiData.SelectedValue.ToString() == "Chỉ số tức thời" || cbLoaiData.SelectedValue.ToString() == "Thông số vận hành")
            {
                pickerTuNgay.IsEnabled = false;
                pickerDenNgay.IsEnabled = false;
                denngay = DateTime.Now;
                pickerDenNgay.Text = denngay.ToString("dd/MM/yyyy");
                tungay = DateTime.Now;
                pickerTuNgay.Text = tungay.ToString("dd/MM/yyyy");
            }
            else
            {
                pickerTuNgay.IsEnabled = true;
                pickerDenNgay.IsEnabled = true;
            }
        }
    }
}
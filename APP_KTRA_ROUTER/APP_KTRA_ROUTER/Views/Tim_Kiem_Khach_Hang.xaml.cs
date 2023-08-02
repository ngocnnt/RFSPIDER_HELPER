using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_KTRA_ROUTER.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tim_Kiem_Khach_Hang : ContentPage
    {
        string _DienLuc = "";
       public  ObservableCollection <TIM_KIEM_KHACH_HANG> ListKhachHang { get; set; }
        public Tim_Kiem_Khach_Hang(string dienluc,string tendienluc)
        {
            InitializeComponent();
            ListKhachHang = new ObservableCollection<TIM_KIEM_KHACH_HANG>();
            Title = tendienluc;
            BindingContext = this;
            _DienLuc = dienluc;
        }

        private async void btnGuiYeuCau_Clicked(object sender, EventArgs e)
        {
            try
            {
                TIM_KIEM_KHACH_HANG tkiem = lstKhachHang.SelectedItem as TIM_KIEM_KHACH_HANG;
                if (tkiem != null)
                {
                    DCU_ROUTER dcu = new DCU_ROUTER { DcuID = tkiem.ID_DCU, MeterID = tkiem.SERY_CTO, Type = tkiem.METER_TYPE };
                    await Navigation.PushAsync(new CongTo_Check(tkiem.MA_TRAM , tkiem.MA_DVIQLY, dcu));

                }

            }
            catch (Exception ex)
            {
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
        }

        private async void btnSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
               await DependencyService.Get<IProcessLoader>().Show("Đang tìm vui lòng đợi");
                string str = Config.URL + "api/home/TimThongTinKhachHang?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&dienluc=" + _DienLuc + "&tenkhachhang=" + search.Text;
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                 await   DependencyService.Get<IProcessLoader>().Hide();
                    ListKhachHang = JsonConvert.DeserializeObject<ObservableCollection<TIM_KIEM_KHACH_HANG>>(result);
                    lstKhachHang.ItemsSource = ListKhachHang;
                }
                else
                {
                   await DependencyService.Get<IProcessLoader>().Hide();
                    DependencyService.Get<IMessage>().ShortAlert("Không tìm thấy thông tin khách hàng");
                    ListKhachHang.Clear();
                    lstKhachHang.ItemsSource = ListKhachHang;

                }
            }
            catch (Exception)
            {
               await DependencyService.Get<IProcessLoader>().Hide();

            }
            
        }
    }
}
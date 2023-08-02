using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentPage
    {
        public ObservableCollection<DonVi> lstDonVi { get; set; }
        public Setting()
        {
            InitializeComponent();
            try
            {
                var _json = Config.client.GetStringAsync("http://smart.cpc.vn/DCU_ROUTER/api/home/GET_CTY_DIEN_LUC").Result;
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    lstDonVi = JsonConvert.DeserializeObject<ObservableCollection<DonVi>>(result);
                    com.DataSource = lstDonVi;
                    foreach (DonVi donVi in lstDonVi)
                    {
                        if (donVi.MA_DON_VI == Preferences.Get(Config.DonVi, ""))
                        {
                            com.SelectedItem = donVi;
                        }
                    }
                }
                else
                {
                     new MessageBox("Thông Báo", "Không kết nối được với database").Show();
                }

            }
            catch (Exception ex)
            {
                new MessageBox("Lỗi", ex.Message).Show();
            }

        }
        private void com_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                if (com.SelectedIndex != -1)
                {
                    DonVi dv = lstDonVi[com.SelectedIndex];
                    Preferences.Set(Config.DonVi, dv.MA_DON_VI);
                }    
               
            }
            catch (Exception ex)
            {

                new MessageBox("Lỗi", ex.Message).Show();
            }
        }
    }
}
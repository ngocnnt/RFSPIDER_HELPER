using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.ViewModels
{
   public class LichSuViewModel : BaseViewModel
    {
        public string URL_API = "https://smart.cpc.vn/DSPM_Api/";
        public string loai_thay = "";
        ObservableCollection<HISTORY_INFO> _dSLichSu;
        public ObservableCollection<HISTORY_INFO> DSLichSu
        {
            get { return _dSLichSu; }
            set
            {
                _dSLichSu = value;
                OnPropertyChanged("DSLichSu");
            }
        }

        bool _isHide;
        public bool isHide { get => _isHide; set => SetProperty(ref _isHide, value); }
        string _stringCu;
        public string stringCu { get => _stringCu; set => SetProperty(ref _stringCu, value); }
        string _stringMoi;
        public string stringMoi { get => _stringMoi; set => SetProperty(ref _stringMoi, value); }

        public LichSuViewModel(int loai) 
        {
            loai_thay = loai.ToString();
            if (loai == 1)
            {
                stringCu = "IMEI cũ";
                stringMoi = "IMEI mới";
                isHide = false;
            }   
            else
            {
                stringCu = "Serial cũ";
                stringMoi = "Serial mới";
                isHide = true;
            }
            LoadData();
        }

        private async void LoadData()
        {
            await DependencyService.Get<IProcessLoader>().Show("Vui lòng đợi...");
            var _json = Config.client.GetStringAsync(URL_API + "api/modem/getHistory?nguoi_sua=" + Preferences.Get(Config.User, "") + "&tu_ngay=" + "1/1/2023" + "&den_ngay=" + "1/1/2030" + "&loai_thay=" + loai_thay).Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            if (_json.Contains("[]") == false)
            {
                Int32 from = _json.IndexOf("[");
                Int32 to = _json.IndexOf("]");
                string result = _json.Substring(from, to - from + 1);
                var response = JsonConvert.DeserializeObject<ObservableCollection<HISTORY_INFO>>(result);
                DSLichSu = response;
                await DependencyService.Get<IProcessLoader>().Hide();
            }
            else
            {
                await DependencyService.Get<IProcessLoader>().Hide();
            }
        }
    }
}

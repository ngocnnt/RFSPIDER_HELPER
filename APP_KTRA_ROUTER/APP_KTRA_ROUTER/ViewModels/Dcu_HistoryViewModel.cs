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
   public class Dcu_HistoryViewModel : BaseViewModel
    {
        ObservableCollection<DonVi> _DonVis;
        public ObservableCollection<DonVi> DonVis 
        {
            get { return _DonVis; }
            set
            {
                _DonVis = value;
                OnPropertyChanged("DonVis");
            }
        }
        ObservableCollection<TRAM> _trams;
        public ObservableCollection<TRAM> Trams
        {
            get { return _trams; }
            set
            {
                _trams = value;
                OnPropertyChanged("Trams");
            }
        }
        ObservableCollection<History> _dcuRouter;
        public ObservableCollection<History> LstDcuRouter
        {
            get { return _dcuRouter; }
            set
            {
                if (value != _dcuRouter)
                {
                    _dcuRouter = value;
                    OnPropertyChanged(nameof(LstDcuRouter));
                }

            }
        }

        private TRAM _selectItemTram;
        public TRAM SelectItemTram
        {
            get { return _selectItemTram; }
            set
            {

                if (_selectItemTram != value)
                {
                    _selectItemTram = value;
                    LstDcuRouter.Clear();
                    TRAM dv = _selectItemTram as TRAM;
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                               ExecuteLoadDCUsCommand(dv);
                        });
                    }
                    catch (Exception ex)
                    {


                    }
                   
                    OnPropertyChanged("SelectItemTram");
                }

            }
        }

        private DonVi _selectItemDonVi;
        public DonVi SelectItemDonVi
        {
            get
            {

                return _selectItemDonVi;
            }
            set
            {

                if (_selectItemDonVi != value)
                {
                    _selectItemDonVi = value;
                    Trams.Clear();
                    LstDcuRouter.Clear();
                    DonVi dv = _selectItemDonVi as DonVi;
                    Device.BeginInvokeOnMainThread( async () =>
                    {
                       await  ExecuteLoadTramsCommand(dv);
                    });
                   
                    OnPropertyChanged("SelectItemDonVi");
                }

            }
        }

        private DCU_ROUTER _selectItemDcu;
        public DCU_ROUTER SelectItemDcu
        {
            get
            {
                return _selectItemDcu;
            }
            set
            {
                if (_selectItemDcu != value)
                {
                    _selectItemDcu = value;
                    OnPropertyChanged("SelectItemDcu");
                }
            }
        }
        public Command LoadDonvisCommand { get; set; }
        public Command<DonVi> LoadTramsCommand { get; set; }
        public Command<TRAM> LoadDCUCommand { get; set; }
        public Dcu_HistoryViewModel() 
        {            
            DonVis = new ObservableCollection<DonVi>();
            Trams = new ObservableCollection<TRAM>();
            LstDcuRouter = new ObservableCollection<History>();
            LoadDonvisCommand = new Command(async () => await ExecuteLoadDonvisCommand());
            LoadTramsCommand = new Command<DonVi>(async (p) => await ExecuteLoadTramsCommand(p));
            LoadDCUCommand = new Command<TRAM>(async (a) => await ExecuteLoadDCUsCommand(a));

        }

        async Task ExecuteLoadDonvisCommand()
        {
            IsBusy = true;

            try
            {

                DonVis.Clear();
                if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
                {
                    var _json = Config.client.GetStringAsync(Config.URL + "api/home/get_dien_luc?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "")).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        DonVis = JsonConvert.DeserializeObject<ObservableCollection<DonVi>>(result);

                    }
                    else
                    {
                        await new MessageBox("Thông Báo", "Không tìm thấy thông tin theo điện lực").Show();
                    }
                }

            }
            catch (Exception ex)
            {
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadTramsCommand(DonVi donvi)
        {
            IsBusy = true;

            try
            {

                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TRAM?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&ma_dien_luc=" + donvi.MA_DON_VI).Result;
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    Trams = JsonConvert.DeserializeObject<ObservableCollection<TRAM>>(result);
                }
                else
                {
                    await new MessageBox("Thông Báo", "Không tìm thấy thông tin trạm theo điện lực").Show();
                }
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoadDCUsCommand(TRAM tRAM) 
        {
            IsBusy = true;
            try
            {
                await DependencyService.Get<IProcessLoader>().Show("Đang tải dữ liệu vui lòng đợi");
                await Task.Delay(1000);
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_DCU_HISTORY?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&matram=" + tRAM.MA_TRAM ).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");

                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    LstDcuRouter  = JsonConvert.DeserializeObject<ObservableCollection<History>>(result);
                   
                    await DependencyService.Get<IProcessLoader>().Hide();
                }

            }
            catch (Exception ex)
            {
                await DependencyService.Get<IProcessLoader>().Hide();
            }
            finally
            {
                IsBusy = false;
                await DependencyService.Get<IProcessLoader>().Hide();
            }
        }
    }
}

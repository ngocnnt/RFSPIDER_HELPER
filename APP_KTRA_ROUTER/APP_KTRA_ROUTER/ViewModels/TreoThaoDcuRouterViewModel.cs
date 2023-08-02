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
   public class TreoThaoDcuRouterViewModel : BaseViewModel
    {
        ObservableCollection<ThaoTac> _ThaoTacs;
        public ObservableCollection<ThaoTac> ThaoTacs
        {
            get { return _ThaoTacs; }
            set
            {
                _ThaoTacs = value;
                OnPropertyChanged("ThaoTacs");
            }
        }
        ObservableCollection<LyDoThao> _LyDoThaos;
        public ObservableCollection<LyDoThao> LyDoThaos
        {
            get { return _LyDoThaos; }
            set
            {
                _LyDoThaos = value;
                OnPropertyChanged("LyDoThaos");
            }
        }
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

        private bool _isTT;

        public bool IsTT
        {
            get
            {
                return _isTT;
            }
            set
            {
                _isTT = value;
                RaisePropertyChanged("IsTT");
            }
        }

        private bool _isLM;

        public bool IsLM
        {
            get
            {
                return _isLM;
            }
            set
            {
                _isLM = value;
                RaisePropertyChanged("IsLM");
            }
        }

        private bool _isTH;

        public bool IsTH
        {
            get
            {
                return _isTH;
            }
            set
            {
                _isTH = value;
                RaisePropertyChanged("IsTH");
            }
        }

        private void RaisePropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        private ThaoTac _selectItemThaoTac;
        public ThaoTac SelectItemThaoTac
        {
            get { return _selectItemThaoTac; }
            set
            {

                if (_selectItemThaoTac != value)
                {
                    _selectItemThaoTac = value;
                    LstDcuRouter.Clear();
                    ThaoTac tc = _selectItemThaoTac as ThaoTac;
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (tc.LOAI_THAO_TAC == "TT")
                            {
                                _isTT = true;
                                _isLM = false;
                                _isTH = false;
                            } else if (tc.LOAI_THAO_TAC == "LM")
                            {
                                _isTT = false;
                                _isLM = true;
                                _isTH = false;
                            }
                            else if (tc.LOAI_THAO_TAC == "TH")
                            {
                                _isTT = false;
                                _isLM = false;
                                _isTH = true;
                            } else
                            {
                                _isTT = true;
                                _isLM = true;
                                _isTH = true;
                            }
                        });
                    }
                    catch (Exception ex)
                    {


                    }

                    OnPropertyChanged("SelectItemThaoTac");
                }

            }
        }

        private LyDoThao _selectItemLyDoThao;
        public LyDoThao SelectItemLyDoThao
        {
            get { return _selectItemLyDoThao; }
            set
            {

                if (_selectItemLyDoThao != value)
                {
                    _selectItemLyDoThao = value;
                    LyDoThao tc = _selectItemLyDoThao as LyDoThao;


                    OnPropertyChanged("SelectItemLyDoThao");
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
                    //Trams.Clear();
                    //LstDcuRouter.Clear();
                    //DonVi dv = _selectItemDonVi as DonVi;
                    //Device.BeginInvokeOnMainThread( async () =>
                    //{
                    //   await  ExecuteLoadTramsCommand(dv);
                    //});
                   
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
        public Command<DonVi> LoadTramsCCChuaKBaoCommand { get; set; }
        public Command<DonVi> LoadTramsCDChuaKBaoCommand { get; set; }
        public Command<TRAM> LoadDCUCommand { get; set; }
        public TreoThaoDcuRouterViewModel() 
        {
            ThaoTacs = new ObservableCollection<ThaoTac>();
            ThaoTacs.Add(new ThaoTac("TT", "Thay thế"));
            ThaoTacs.Add(new ThaoTac("LM", "Lắp mới"));
            ThaoTacs.Add(new ThaoTac("TH", "Tháo"));
            LyDoThaos = new ObservableCollection<LyDoThao>();
            LyDoThaos.Add(new LyDoThao("1"));
            LyDoThaos.Add(new LyDoThao("2"));
            DonVis = new ObservableCollection<DonVi>();
            Trams = new ObservableCollection<TRAM>();
            LstDcuRouter = new ObservableCollection<History>();
            LoadDonvisCommand = new Command(async () => await ExecuteLoadDonvisCommand());
            LoadTramsCommand = new Command<DonVi>(async (p) => await ExecuteLoadTramsCommand(p));
            LoadTramsCCChuaKBaoCommand = new Command<DonVi>(async (p) => await ExecuteLoadTramsCCChuaKBaoCommand(p));
            LoadTramsCDChuaKBaoCommand = new Command<DonVi>(async (p) => await ExecuteLoadTramsCDChuaKBaoCommand(p));
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
                Trams.Clear();
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

        async Task ExecuteLoadTramsCCChuaKBaoCommand(DonVi donvi)
        {
            IsBusy = true;

            try
            {
                Trams.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TRAM_CC_CHUA_KBAO?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&ma_dien_luc=" + donvi.MA_DON_VI).Result;
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

        async Task ExecuteLoadTramsCDChuaKBaoCommand(DonVi donvi)
        {
            IsBusy = true;

            try
            {
                Trams.Clear();
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TRAM_CD_CHUA_KBAO?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&ma_dien_luc=" + donvi.MA_DON_VI).Result;
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

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Views;
using APP_KTRA_ROUTER.Global;
using Newtonsoft.Json;
using APP_KTRA_ROUTER.Popup;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Services;
using APP_KTRA_ROUTER.Database;
using System.Collections.Generic;
using System.Linq;

namespace APP_KTRA_ROUTER.ViewModels
{
    public class KiemTraViewModel : BaseViewModel
    {
        public Save_PathReposity Savepath_Reposity { get; set; } 
        ObservableCollection<TIM_CONG_TO> _timCongTo;
        public ObservableCollection<TIM_CONG_TO> TimCongTos
        {
            get { return _timCongTo; }
            set
            {
                _timCongTo = value;
                OnPropertyChanged("TimCongTos");
            }
        }


        ObservableCollection<DonVi> _DonVis;
        public ObservableCollection<DonVi> DonVis
        {
            get { return _DonVis; }
            set
            {
                if (value != null )
                {
                    _DonVis = value;
                    OnPropertyChanged("DonVis");
                }    
               
            }
        }
         ObservableCollection<TRAM> _trams;
        public ObservableCollection<TRAM> Trams
        {
            get { return _trams; }
            set
            {
                if (value != null )
                {
                    _trams = value;
                    OnPropertyChanged("Trams");
                }                   
            }
        }
        ObservableCollection<PATH> _paths; 
        public ObservableCollection<PATH> LstPaths
        {
            get { return _paths; }
            set
            {
                if (value != null )
                {
                    _paths = value;
                    OnPropertyChanged(nameof(LstPaths));
                }    
                

            }
        }

        ObservableCollection<ChungLoai> _chungloai;
        public ObservableCollection<ChungLoai> LstChungLoai
        {
            get { return _chungloai; }
            set
            {
                if (value != null )
                {
                    _chungloai = value;
                    OnPropertyChanged(nameof(LstChungLoai));
                }    
            }
        }

        private  TRAM _selectItemTram;
        public TRAM SelectItemTram
        {
            get { return _selectItemTram; }
            set
            {
                if (value != null)
                {
                    _selectItemTram = value;
                    LstPaths.Clear();
                    TRAM dv = _selectItemTram as TRAM;
                    Task.Run(async () => await ExecuteLoadPathsCommand(dv)).Wait();
                    OnPropertyChanged("SelectItemTram");
                    OnPropertyChanged("LstPaths");
                }    
                

            }
        }

        private DonVi _selectItemDonVi;
        public DonVi SelectItemDonVi
        {
            get { 
                
                return _selectItemDonVi; }
            set
            {
                    _selectItemDonVi = value;
                    Trams.Clear();
                    LstPaths.Clear();
                    DonVi dv = _selectItemDonVi as DonVi;
                    Task.Run(async () => await ExecuteLoadTramsCommand(dv)).Wait();
                    OnPropertyChanged("SelectItemDonVi");
            }
        }
        private ChungLoai _selectChungLoai;
        public ChungLoai SelectChungLoai
        {
            get
            {
                return _selectChungLoai;
            }
            set
            {
                if (value != null )
                {
                    _selectChungLoai = value;
                    OnPropertyChanged("SelectChungLoai");
                }    
               
            }
        }

        private PATH _selectItemPath;
        public PATH SelectItemPath
        {
            get
            {
                return _selectItemPath;
            }
            set
            {
                if (value != null )
                {
                    _selectItemPath = value;
                    OnPropertyChanged("SelectItemPath");
                }    
               
            }
        }
        public Command LoadDonvisCommand { get; set; }
        public Command<string> TimCongToCommand { get; set; }
        public Command<DonVi> LoadTramsCommand { get; set; }
        public Command<TRAM> LoadPathsCommand { get; set; }
        public Command LoadChungLoaisCommand { get; set; }
        public KiemTraViewModel()
        {
            Title = "Khám phá đường dẫn...";
            DonVis = new ObservableCollection<DonVi>();
            Trams = new ObservableCollection<TRAM>();
            LstPaths = new ObservableCollection<PATH>();
            TimCongTos = new ObservableCollection<TIM_CONG_TO>();
            Savepath_Reposity = new Save_PathReposity(App.gCS_Dbcontext);
            LoadDonvisCommand = new Command(async () => await ExecuteLoadDonvisCommand());
            LoadTramsCommand = new Command<DonVi>(async (p) => await ExecuteLoadTramsCommand(p));
            LoadPathsCommand = new Command<TRAM >(async (a) => await ExecuteLoadPathsCommand(a));
            LoadChungLoaisCommand = new Command(async () => await ExecuteLoaChungLoaiCommand());
            TimCongToCommand = new Command<string>(async (p) => await ExecuteTimCongToCommand(p));
        }

        private async Task ExecuteTimCongToCommand(string socongto)
        {          

            try
            {
                await DependencyService.Get<IProcessLoader>().Show("Đang kiểm tra....");
                await Task.Delay(1000);
                if  (Xamarin.Essentials.Preferences.Get(Config.DonVi,"") != "")
                {
                    string str = Config.URL + "api/home/TIM_CONG_TO?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&serial=" + socongto;
                    var _json = Config.client.GetStringAsync(str  ).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);
                        TimCongTos = JsonConvert.DeserializeObject<ObservableCollection<TIM_CONG_TO>>(result);
                        foreach (TIM_CONG_TO cto in TimCongTos )
                        {
                            //ktra xem thuộc đơn vị nào
                            foreach (DonVi dv in DonVis )
                            {
                                if (cto.MA_DVIQLY == dv.MA_DON_VI )
                                {

                                    Debug.WriteLine($"find don vi {dv.MA_DON_VI}");
                                    SelectItemDonVi = dv;
                                    break;
                                }    
                            }
                            foreach (TRAM tr in Trams)
                            {
                                if (cto.MA_TRAM == tr.MA_TRAM)
                                {
                                    SelectItemTram = tr ;
                                    Debug.WriteLine($"find tram {tr.MA_TRAM}");
                                    break;
                                }
                            }
                            foreach (ChungLoai  cl in LstChungLoai)
                            {
                                if (cto.METER_TYPE == cl.METER_TYPE)
                                {
                                    SelectChungLoai = cl;
                                    break;
                                }
                            }
                            bool findPath = false;

                            var slItemPath = LstPaths.FirstOrDefault(d => d.SPIDER_PATH == cto.SPIDER_PATH);
                            //the same as below code
                            foreach (PATH pt in LstPaths)
                            {
                                if (cto.SPIDER_PATH == pt.SPIDER_PATH)
                                {
                                    SelectItemPath = pt;
                                    findPath = true;
                                    break;
                                }
                            }
                            if (findPath == false )
                            {
                                PATH newPath = new PATH { SPIDER_PATH = cto.SPIDER_PATH };
                                LstPaths.Add(newPath);
                                SelectItemPath = newPath;
                                OnPropertyChanged("LstPaths");
                                OnPropertyChanged("SelectItemPath");
                            }    
                        }    
                    }
                    else
                    {
                        await DependencyService.Get<IProcessLoader>().Hide();
                        DependencyService.Get<IMessage>().ShortAlert("Không tìm thấy số sery này trong hệ thống");
                        //await new MessageBox("Thông Báo", "Không tìm thấy thông tin công tơ này trong hệ thống").Show();
                    }
                }

            }
            catch (Exception ex)
            {
                await DependencyService.Get<IProcessLoader>().Hide();
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
            finally
            {               
                await DependencyService.Get<IProcessLoader>().Hide();
            }
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
                        DependencyService.Get<IMessage>().ShortAlert("Không tìm thấy thông tin điện lực");
                        //await new MessageBox("Thông Báo", "Không tìm thấy thông tin theo điện lực").Show();
                    }
                }

            }
            catch (Exception ex)
            {
                await new MessageBox("Thông Báo", ex.Message ).Show();
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

                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TRAM?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&ma_dien_luc=" + donvi.MA_DON_VI ).Result;
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
                    DependencyService.Get<IMessage>().ShortAlert("Không tìm thấy trạm");
                    //await new MessageBox("Thông Báo", "Không tìm thấy thông tin trạm theo điện lực").Show();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteLoadPathsCommand(TRAM tRAM) 
        {
            IsBusy = true;
            try
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_PATH_DCU?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&id_Dcu=" + tRAM.ID_DCU ).Result;
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    LstPaths = JsonConvert.DeserializeObject<ObservableCollection<PATH>>(result);
                    //lấy path đã lưu ở db ra
                    //List<Save_Path> paths = await Savepath_Reposity.GetAllPath_TramAsync(SelectItemTram.MA_TRAM);
                    //foreach (Save_Path path in paths)
                    //{
                    //    LstPaths.Add(new PATH { SPIDER_PATH = path.Path });
                    //}    
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

        private async Task ExecuteLoaChungLoaiCommand()
        {
            IsBusy = true;
            try
            {
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_CHUNG_LOAI?congty=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") ).Result;
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    LstChungLoai = JsonConvert.DeserializeObject<ObservableCollection<ChungLoai >>(result);
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
    }
}
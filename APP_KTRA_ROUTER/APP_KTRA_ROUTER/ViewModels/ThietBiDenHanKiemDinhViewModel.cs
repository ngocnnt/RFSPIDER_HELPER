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
   public class ThietBiDenHanKiemDinhViewModel : BaseViewModel
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
        ObservableCollection<string> _nams;
        public ObservableCollection<string> Nams
        {
            get { return _nams; }
            set
            {
                _nams = value;
                OnPropertyChanged("Trams");
            }
        }
        ObservableCollection<TTinKDinh> _dcuRouter;
        public ObservableCollection<TTinKDinh> LstDcuRouter
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

        private string _selectItemNam;
        public string SelectItemNam
        {
            get { return _selectItemNam; }
            set
            {

                if (_selectItemNam != value)
                {
                    _selectItemNam = value;
                    LstDcuRouter.Clear();
                    string dv = _selectItemNam as string;
                    try
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                               //ExecuteLoadDCUsCommand(dv);
                        });
                    }
                    catch (Exception ex)
                    {


                    }
                   
                    OnPropertyChanged("SelectItemNam");
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
                    //Nams.Clear();
                    LstDcuRouter.Clear();
                    DonVi dv = _selectItemDonVi as DonVi;
                    Device.BeginInvokeOnMainThread( async () =>
                    {
                       await  ExecuteLoadNamsCommand();
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
        public Command LoadNamsCommand { get; set; }
        public Command<string> LoadDCUCommand { get; set; }
        public ThietBiDenHanKiemDinhViewModel() 
        {            
            DonVis = new ObservableCollection<DonVi>();
            Nams = new ObservableCollection<string>();
            LstDcuRouter = new ObservableCollection<TTinKDinh>();
            LoadDonvisCommand = new Command(async () => await ExecuteLoadDonvisCommand());
            LoadNamsCommand = new Command(async () => await ExecuteLoadNamsCommand());
            LoadDCUCommand = new Command<string>(async (t) => await ExecuteLoadDCUsCommand(t));

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
                        DonVi item = new DonVi();
                        item.MA_DON_VI = Xamarin.Essentials.Preferences.Get(Config.DonVi, "");
                        item.TEN_DON_VI = item.MA_DON_VI;
                        DonVis.Insert(0,item);
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

        async Task ExecuteLoadNamsCommand()
        {
            IsBusy = true;

            try
            {
                for(int i=2000;i<2050;i++)
                {
                    Nams.Add(i.ToString());
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

        private async Task ExecuteLoadDCUsCommand(string thangnam) 
        {
            IsBusy = true;
            try
            {
                await DependencyService.Get<IProcessLoader>().Show("Đang tải dữ liệu vui lòng đợi");
                await Task.Delay(1000);
                var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TBI_DENHAN_KIEMDINH?ma_dien_luc=" + thangnam.Split('/')[0] + "&loai_tbi=" + thangnam.Split('/')[1] + "&thang=" + thangnam.Split('/')[2] + "&nam=" + thangnam.Split('/')[3]).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    LstDcuRouter  = JsonConvert.DeserializeObject<ObservableCollection<TTinKDinh>>(result);
                   foreach(var item in LstDcuRouter)
                    {
                        item.NOT_DA_THAO = !item.DA_THAO;
                    }
                }
                await DependencyService.Get<IProcessLoader>().Hide();
                if (LstDcuRouter.Count < 1)
                {
                    await new MessageBox("Thông Báo", "Không Tìm Thấy Dữ Liệu").Show();
                }
            }
            catch (Exception ex)
            {
                await DependencyService.Get<IProcessLoader>().Hide();
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
                await DependencyService.Get<IProcessLoader>().Hide();
            }
        }
    }
}

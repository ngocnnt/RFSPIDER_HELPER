using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Interface;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace APP_KTRA_ROUTER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DSKiemDinhBaoHanh : ContentPage
    {
        string ma_dien_luc = "";
        string ma_tram = "";
        ObservableCollection<KIEM_DINH_BAO_HANH> _lstDCU { get; set; }
        APP_KTRA_ROUTER.ViewModels.DSKiemDinhBaoHanhViewModel viewModel;
        public DSKiemDinhBaoHanh()

        {
            InitializeComponent();

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") == "")
            {
                Navigation.PushAsync(new Setting());
            }
            BindingContext = viewModel = new ViewModels.DSKiemDinhBaoHanhViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.IsBusy == true) return;
            if (viewModel.DonVis.Count == 0)
                viewModel.LoadDonvisCommand.Execute(null);
            if (viewModel.DonViCLoais.Count == 0)
                viewModel.LoadDonViCLoaisCommand.Execute(null);
            viewModel.LoadNamsCommand.Execute(null);
            search.Text = "";
            cbTinhTrang.SelectedIndex = 1;
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                //Best practice to always check that the key exists
                if (results.ContainsKey(Permission.Storage))
                    status = results[Permission.Storage];
            }
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
        private async void btnThem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (cbChungLoai.SelectedIndex == -1 || cbChungLoai.SelectedItem.ToString() == "")
                {
                    await new MessageBox("thông báo", "Vui lòng chọn chủng loại thiết bị.").Show();
                    return;
                }
                if (search.Text.Trim() != "") { } else
                {
                    await new MessageBox("thông báo", "Vui lòng nhập số serial hoặc quét barcode.").Show();
                    return;
                }
                if (viewModel.LstDcuRouter.Where(x=>x.ID == search.Text).ToList().Count() == 1)
                {
                    await new MessageBox("thông báo", "Thiết bị đã tồn tại trong danh sách.").Show();
                } else
                {
                    //var result = await this.DisplayAlert("Xác nhận!", "Thêm thiết bị?", "Yes", "No");
                    //if (result)
                    {
                        KIEM_DINH_BAO_HANH item = new KIEM_DINH_BAO_HANH();
                        item.ID = search.Text;
                        item.CHUNG_LOAI = ((DonViCLoai)cbChungLoai.SelectedItem).MA_CLOAI;
                        item.TEN_CHUNG_LOAI = ((DonViCLoai)cbChungLoai.SelectedItem).TEN_CLOAI;
                        item.TEN_TINH_TRANG = cbTinhTrang.SelectedItem.ToString();
                        if (item.TEN_TINH_TRANG == "") item.TINH_TRANG = "1";
                        else if (item.TEN_TINH_TRANG == "Ngập lụt") item.TINH_TRANG = "2";
                        item.MO_TA_LOI = motaloi.Text;

                        var _json = Config.client.PostAsync(Config.URL + "api/home/GetNgayXuatKhoBySerialIdAsync?serialId="+item.ID+"&maChungLoai="+item.CHUNG_LOAI, null).Result;
                        var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").Replace("\"","").ToLower();


                        if (content.Length == 10 && int.Parse(content.Substring(6, 4)) == 1990)
                        {
                            await new MessageBox("Thông Báo", "Không tìm thấy thiết bị").Show();
                            return;
                        }
                        if (content.Length == 10 && int.Parse(content.Substring(6, 4)) == 1980)
                        {
                            await new MessageBox("Thông Báo", "Công tơ đã được thanh lý").Show();
                            return;
                        }
                        if (content.Length != 10)
                        {
                            await new MessageBox("Thông Báo", "Không tìm thấy thiết bị").Show();
                            return;
                        }
                        item.NGAY_XUAT_KHO = content;
                        item.HAN_BAO_HANH = new DateTime(int.Parse(content.Substring(6, 4)), int.Parse(content.Substring(3, 2)), int.Parse(content.Substring(0, 2))).AddMonths(30).ToString("dd/MM/yyyy");
                        viewModel.LstDcuRouter.Add(item);
                    }
                    
                }
            }
            catch (Exception ex)
            {

                await new MessageBox("thông báo", ex.Message + ex.StackTrace).Show();
            }
        }
        private async void scanBtn_ClickedAsync(object sender, EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    search.Text = result.Text;
                });

            };
        }

        private async void btnXuatExcel_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Create an instance of ExcelEngine.
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    //Set the default application version as Excel 2013.
                    excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2013;

                    //Create a workbook with a worksheet
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);

                    //Access first worksheet from the workbook instance.
                    IWorksheet worksheet = workbook.Worksheets[0];

                    //Adding text to a cell
                    worksheet.Range["A1"].Text = "Số serialID";
                    worksheet.Range["B1"].Text = "Chủng loại";
                    worksheet.Range["C1"].Text = "Tình trạng";
                    worksheet.Range["D1"].Text = "Mô tả lỗi";
                    worksheet.Range["E1"].Text = "Ngày xuất kho";
                    worksheet.Range["F1"].Text = "Hạn bảo hành";

                    for(int i=0;i< viewModel.LstDcuRouter.Count;i++)
                    {
                        worksheet.Range["A" + (i + 2)].Text = viewModel.LstDcuRouter[i].ID;
                        worksheet.Range["B" + (i + 2)].Text = viewModel.LstDcuRouter[i].TEN_CHUNG_LOAI;
                        worksheet.Range["C" + (i + 2)].Text = viewModel.LstDcuRouter[i].TEN_TINH_TRANG;
                        worksheet.Range["D" + (i + 2)].Text = viewModel.LstDcuRouter[i].MO_TA_LOI;
                        worksheet.Range["E" + (i + 2)].Text = viewModel.LstDcuRouter[i].NGAY_XUAT_KHO;
                        worksheet.Range["F" + (i + 2)].Text = viewModel.LstDcuRouter[i].HAN_BAO_HANH;
                    }

                    //Save the workbook to stream in xlsx format. 
                    MemoryStream stream = new MemoryStream();
                    workbook.SaveAs(stream);

                    workbook.Close();

                    //Save the stream as a file in the device and invoke it for viewing
                    string ret = await DependencyService.Get<ISaveAndroid>().SaveAndViewAsync("DSBaoHanh_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", "application/msexcel", stream);
                    if (ret != "")
                    {
                        await new MessageBox("thông báo", ret).Show();
                    }
                }

            } catch (Exception ex)
            {
                await new MessageBox("thông báo", ex.Message).Show();
                await new MessageBox("thông báo", ex.StackTrace).Show();
                await new MessageBox("thông báo", ex.Source).Show();
                await new MessageBox("thông báo", ex.HelpLink).Show();
            }
        }
        
        private async void btnGui_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Gửi biên bản", "Ghi chú");

            BienBanCrudApiModel req = new BienBanCrudApiModel();
            req.ListChiTietBienBan = new List<ChiTietBienBanCrudApiModel>();
            req.MaDonVi = Xamarin.Essentials.Preferences.Get(Config.DonVi, "");
            
            if (((DonVi)cbDienLuc.SelectedItem).MA_DON_VI != Xamarin.Essentials.Preferences.Get(Config.DonVi, ""))
            {
                req.MaDVDL = ((DonVi)cbDienLuc.SelectedItem).MA_DON_VI;
            } else
            {
                req.MaDVDL = "";
            }
            req.SoLuong = viewModel.LstDcuRouter.Count;
            req.NguoiTao = Xamarin.Essentials.Preferences.Get(Config.User, "");
            req.TrangThai = "0";
            req.GhiChu = result;
            foreach(KIEM_DINH_BAO_HANH item in viewModel.LstDcuRouter)
            {
                ChiTietBienBanCrudApiModel tmp = new ChiTietBienBanCrudApiModel();
                tmp.SerialId = decimal.Parse(item.ID);
                tmp.MaChungLoai = item.CHUNG_LOAI;
                //tmp.NgayXuatKho = item.NGAY_XUAT_KHO;
                //tmp.HanBaoHanh = item.HAN_BAO_HANH;
                tmp.MoTa = item.MO_TA_LOI == null ? "" : item.MO_TA_LOI;
                tmp.TinhTrang = int.Parse(item.TINH_TRANG);
                req.ListChiTietBienBan.Add(tmp);
            }

            if (Xamarin.Essentials.Preferences.Get(Config.DonVi, "") != "")
            {
                
                HttpContent httpcontent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                var _json = Config.client.PostAsync(Config.URL + "api/home/SEND_VTTB", httpcontent).Result;
                var content = _json.Content.ReadAsStringAsync().Result.Replace("\\r\\n", "").Replace("\\", "").ToLower();
                await new MessageBox("Thông Báo", content).Show();
               
            }
        }

        private async void btnXoa_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (listviewDCU.SelectedIndex > 0)
                {
                    var result = await this.DisplayAlert("Xác nhận!", "Xóa thiết bị?", "Yes", "No");
                    if (result)
                    {
                        //DCU_ROUTER dCU = listviewDCU.SelectedItem as DCU_ROUTER;
                        viewModel.LstDcuRouter.RemoveAt(listviewDCU.SelectedIndex - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                //await new MessageBox("Thông Báo", ex.Message).Show();
            }
        }
    }
}
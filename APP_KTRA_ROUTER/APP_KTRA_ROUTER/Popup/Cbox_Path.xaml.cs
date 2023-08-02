using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_KTRA_ROUTER.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cbox_Path : PopupPage
    {
        TaskCompletionSource<PATH> _tsk = null;
        public ObservableCollection<PATH> _ListPath { get; set; } 
        public Cbox_Path(string serial) 
        {
            InitializeComponent();
            var _json = Config.client.GetStringAsync(Config.URL + "api/home/get_path?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&serial=" + serial ).Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            Int32 from = _json.IndexOf("[");
            Int32 to = _json.IndexOf("]");
            string result = _json.Substring(from, to - from + 1);
            ObservableCollection<PATH> listpath = JsonConvert.DeserializeObject<ObservableCollection<PATH>>(result);
            listViewPath.ItemsSource = _ListPath = listpath;
        }
        public async Task<PATH> Show()
        {
            _tsk = new TaskCompletionSource<PATH>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }
         
        private async void listTram_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PATH _path = e.SelectedItem as PATH;
            await Navigation.PopAllPopupAsync(true);
            _tsk.SetResult(_path);
        }
       

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
        }
        
    }
}
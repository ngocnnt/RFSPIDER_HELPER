using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public partial class Cbox_Tram : ContentPage
    {
        TaskCompletionSource<TRAM> _tsk = null;
        public ObservableCollection<TRAM> _ListTram { get; set; }
        public Cbox_Tram(string ma_dien_luc)
        {
            InitializeComponent();
            var _json = Config.client.GetStringAsync(Config.URL + "api/home/GET_TRAM?donvi=" + Xamarin.Essentials.Preferences.Get(Config.DonVi, "") + "&ma_dien_luc=" + ma_dien_luc).Result;
            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
            Int32 from = _json.IndexOf("[");
            Int32 to = _json.IndexOf("]");
            string result = _json.Substring(from, to - from + 1);
            _ListTram = new ObservableCollection<TRAM>();
            _ListTram  = JsonConvert.DeserializeObject<ObservableCollection<TRAM>>(result);
            listViewTram.ItemsSource =_ListTram;
        }
        
        

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string matram = search.Text;
                if (matram != "")
                {
                    listViewTram.ItemsSource = _ListTram.Where(p => p.TEN_TRAM.ToLower().Contains(matram.ToLower()) ).ToList();
                }
                else
                {
                    listViewTram.ItemsSource = _ListTram;
                }
            }
            catch (Exception ex)
            {

               
            }
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           await Navigation.PopModalAsync();
        }

        private void listViewTram_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
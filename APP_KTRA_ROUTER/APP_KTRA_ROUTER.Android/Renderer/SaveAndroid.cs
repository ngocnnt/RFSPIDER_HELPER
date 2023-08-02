using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using APP_KTRA_ROUTER.Droid.Renderer;
using APP_KTRA_ROUTER.Interface;
using Java.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
[assembly: Dependency(typeof(SaveAndroid))]
namespace APP_KTRA_ROUTER.Droid.Renderer
{
    class SaveAndroid : ISaveAndroid
    {
        //Method to save document as a file in Android and view the saved document
        [Obsolete]
        public async Task<string> SaveAndViewAsync(string fileName, String contentType, MemoryStream stream)//, AppCompatActivity activity)
        {
            try
            {
                string root = null;
                //Get the root path in android device.

                ////var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                ////if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                ////{
                ////    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                ////    {
                ////    }

                ////    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                ////    //Best practice to always check that the key exists
                ////    if (results.ContainsKey(Permission.Storage))
                ////        status = results[Permission.Storage];
                ////}
                if (Android.OS.Environment.IsExternalStorageEmulated)
                {
                    root = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString() + "/Download";
                }
                else
                    root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

                //Create directory and file 
                Java.IO.File myDir = new Java.IO.File(root);
                myDir.Mkdir();

                Java.IO.File file = new Java.IO.File(myDir, fileName);

                //Remove if the file exists
                if (file.Exists()) file.Delete();

                FileOutputStream writer = new FileOutputStream(file);
                writer.Write(stream.ToArray());
                writer.Flush();
                writer.Close();

                return file.Path;
            } catch (Exception ex) { return ex.Message; }
        }
    }
}
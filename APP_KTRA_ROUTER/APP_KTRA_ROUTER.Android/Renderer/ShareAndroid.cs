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
[assembly: Dependency(typeof(ShareAndroid))]
namespace APP_KTRA_ROUTER.Droid.Renderer
{
    class ShareAndroid : IShare
    {
        private readonly Context context;
        public ShareAndroid()
        {
            context = Android.App.Application.Context;
        }
        public Task Show(string title, string message, string fileName)
        {
            var uri = Android.Net.Uri.Parse("file://" + fileName);
            Java.IO.File file = new Java.IO.File(fileName);
            Android.Net.Uri photoURI = FileProvider.GetUriForFile(context, "com.companyname.app_ktra_router.provider", file);
            var contentType = "application/excel";
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraStream, uri);
            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);
            intent.SetType(contentType);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using APP_KTRA_ROUTER.Droid.Renderer;
using APP_KTRA_ROUTER.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ImageEntry), typeof(ImageEntryRenderer))]
namespace APP_KTRA_ROUTER.Droid.Renderer
{
    public class ImageEntryRenderer : EntryRenderer
    {
        ImageEntry element;
        public ImageEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (ImageEntry)this.Element;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(element.ImageSecond))
            {
                switch (element.ImageSecondAlignment)
                {
                    case ImageAlignment.Left:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.ImageSecond), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.ImageSecond), null);
                        break;
                }
            }
            Control.CompoundDrawablePadding = 25;
            var gradienDrawble = new GradientDrawable();
            gradienDrawble.SetCornerRadius(60f);
            gradienDrawble.SetStroke(5, Android.Graphics.Color.DeepPink);
            gradienDrawble.SetColor(Android.Graphics.Color.LightGray);
            Control.SetBackground(gradienDrawble);
            Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight, Control.PaddingBottom);

        }


        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * element.ImageSize, element.ImageHeight * element.ImageSize, true));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace APP_KTRA_ROUTER.Renderer
{
    public class ImageEntry : Entry
    {
        public ImageEntry()
        {
            // this.HeightRequest = 50;
        }
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(ImageEntry), string.Empty);
        public static readonly BindableProperty ImageSecondProperty =
           BindableProperty.Create(nameof(ImageSecondProperty), typeof(string), typeof(ImageEntry), string.Empty);

        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create(nameof(LineColor), typeof(Xamarin.Forms.Color), typeof(ImageEntry), Color.White);

        public static readonly BindableProperty ImageHeightProperty =
            BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(ImageEntry), 40);

        public static readonly BindableProperty ImageWidthProperty =
            BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(ImageEntry), 40);

        public static readonly BindableProperty ImageAlignmentProperty =
            BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(ImageEntry), ImageAlignment.Left);
        public static readonly BindableProperty ImageSecondAlignmentProperty =
          BindableProperty.Create(nameof(ImageSecondAlignment), typeof(ImageAlignment), typeof(ImageEntry), ImageAlignment.Left);

        public static readonly BindableProperty ImageSizeProperty = BindableProperty.Create(nameof(ImageSize), typeof(int), typeof(ImageEntry), 1);

        public static readonly BindableProperty ClearButtonProperty = BindableProperty.Create(nameof(ClearButton), typeof(Boolean), typeof(ImageEntry), false);

        public Boolean ClearButton
        {
            get { return (Boolean)GetValue(ClearButtonProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
        public int ImageSize
        {
            get { return (int)GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }
        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public string ImageSecond
        {
            get { return (string)GetValue(ImageSecondProperty); }
            set { SetValue(ImageSecondProperty, value); }
        }

        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }
        public ImageAlignment ImageSecondAlignment
        {
            get { return (ImageAlignment)GetValue(ImageSecondAlignmentProperty); }
            set { SetValue(ImageSecondAlignmentProperty, value); }
        }
    }

    public enum ImageAlignment
    {
        Left,
        Right
    }
}

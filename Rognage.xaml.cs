using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;


namespace Wave_Lab
{
    public partial class Rognage : MetroWindow
    {
        CroppingAdorner _clp;
        FrameworkElement _felCur = null;
        System.Windows.Media.Brush _brOriginal;

        public Rognage()
        {
            InitializeComponent();           
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public static Bitmap ConvertToBitmap(BitmapSource bitmapSource)
        {
            var width = bitmapSource.PixelWidth;
            var height = bitmapSource.PixelHeight;
            var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, memoryBlockPointer);
            return bitmap;
        }

        public Bitmap currentImage
        {
            get
            {
               
                return ConvertToBitmap(_clp.BpsCrop());
            }
            set
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                value.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                BitmapImage bmapImage = new BitmapImage();
                bmapImage.BeginInit();
                bmapImage.StreamSource = stream;
                bmapImage.EndInit();
                img.Source = bmapImage;
            }
        }

		private void RemoveCropFromCur()
		{
			AdornerLayer aly = AdornerLayer.GetAdornerLayer(_felCur);
			aly.Remove(_clp);
		}

		private void AddCropToElement(FrameworkElement fel)
		{
			if (_felCur != null)
			{
				RemoveCropFromCur();
			}
            
			Rect rcInterior = new Rect(
				fel.ActualWidth * 0.2,
				fel.ActualHeight * 0.2,
				fel.ActualWidth * 0.6,
				fel.ActualHeight * 0.6);
			AdornerLayer aly = AdornerLayer.GetAdornerLayer(fel);
			_clp = new CroppingAdorner(fel, rcInterior);
			aly.Add(_clp);
			imgCrop.Source = _clp.BpsCrop();
			_clp.CropChanged += CropChanged;
			_felCur = fel;
            SetClipColorGrey();
			
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			AddCropToElement(img);
			_brOriginal = _clp.Fill;
			RefreshCropImage();
		}

		private void RefreshCropImage()
		{
			if (_clp != null)
			{
				Rect rc = _clp.ClippingRectangle;

				imgCrop.Source = _clp.BpsCrop();
			}
		}

		private void CropChanged(Object sender, RoutedEventArgs rea)
		{
			RefreshCropImage();
		}

		private void CropImage_Checked(object sender, RoutedEventArgs e)
		{
			if (dckControls != null && img != null)
			{
				dckControls.Visibility = Visibility.Hidden;
				AddCropToElement(img);
				RefreshCropImage();
			}
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			RefreshCropImage();
		}

		private void SetClipColorGrey()
		{
			if (_clp != null)
			{
				System.Windows.Media.Color clr = Colors.Black;
				clr.A = 110;
				_clp.Fill = new SolidColorBrush(clr);
			}
		}

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

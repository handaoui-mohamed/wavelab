using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wave_Lab
{

    public partial class BitmapViewWindow : Window
    {
        CurrentImageHandler currImageHandler;

        public BitmapViewWindow(CurrentImageHandler currImageHandler)
        {
            InitializeComponent();
            this.currImageHandler = currImageHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            currImageHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            BitmapImage bmapImage = new BitmapImage();
            bmapImage.BeginInit();
            bmapImage.StreamSource = stream;
            bmapImage.EndInit();
            MainImage.Source = bmapImage;
            MainImage.Stretch = Stretch.Uniform;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}

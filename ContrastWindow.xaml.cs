using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace Wave_Lab
{
    /// <summary>
    /// Interaction logic for ContrastWindow.xaml
    /// </summary>
    public partial class ContrastWindow : MetroWindow
    {
        private CurrentImageHandler currImgHandler = new CurrentImageHandler();
        private System.Drawing.Bitmap bitmapImg;
        public ContrastWindow()
        {
            InitializeComponent();
            
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void PaintImage()
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            currImgHandler.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, Convert.ToInt32(stream.Length));
            BitmapImage bmapImage = new BitmapImage();
            bmapImage.BeginInit();
            bmapImage.StreamSource = stream;
            bmapImage.EndInit();
            image.Source = bmapImage;
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public System.Drawing.Bitmap bitmap
        {
            get
            {
                return bitmapImg;
            }
            set 
            { 
                bitmapImg = value;
                currImgHandler.CurrentBitmap = value;
            }
        }


        public float ContrastValue
        {
            get
            {
                if (string.IsNullOrEmpty(txtContrastValue.Text))
                    txtContrastValue.Text = "0";
                return Convert.ToSingle(txtContrastValue.Text);
            }
            set { txtContrastValue.Text = value.ToString(); }
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




        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtContrastValue.Text = Convert.ToInt32(slider.Value).ToString();
            currImgHandler.CurrentBitmap = bitmapImg;
            currImgHandler.CurrentContrastHandler.SetContrast(Convert.ToInt32(slider.Value));
            PaintImage();

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PaintImage();
        }
    }
}

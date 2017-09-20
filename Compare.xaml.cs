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
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;

namespace Wave_Lab
{
    public partial class Compare : MetroWindow
    {
        CurrentImageHandler currImgHandler1;
        CurrentImageHandler currImgHandler2;

        public Compare()
        {
            InitializeComponent();
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PaintImage()
        {
            
        }

        private void image1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler1 = new CurrentImageHandler();
                OpenFileDialog ImageDialog = new OpenFileDialog();
                ImageDialog.RestoreDirectory = true;
                ImageDialog.FilterIndex = 1;
                ImageDialog.Filter = "bmp Files (*.bmp)|*.bmp|jpeg Files (*.jpeg)|*.jpeg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |jpg Files (*.jpg)|*.jpg|" + "All files (*.*)|*.*";

                if (ImageDialog.ShowDialog().Value)
                {
                    src1.Text = ImageDialog.FileName;
                    currImgHandler1.CurrentFileHandler.Load(ImageDialog.FileName);
                   FileInfo fileInfo1 = new FileInfo(currImgHandler1.CurrentBitmapPath);
                   if (fileInfo1.Extension == ".bmp")
                   {

                       img1.Source = new BitmapImage(new Uri(currImgHandler1.CurrentBitmapPath));
                   }
                    else
                   {
                       int dev;
                       if (currImgHandler1.CurrentBitmap.Width > currImgHandler1.CurrentBitmap.Height)
                       {
                           dev = currImgHandler1.CurrentBitmap.Width / 193;
                       }
                       else
                       {
                           dev = currImgHandler1.CurrentBitmap.Height / 193;
                       }
                       currImgHandler1.Resize(Convert.ToInt32(currImgHandler1.CurrentBitmap.Width / dev), Convert.ToInt32(currImgHandler1.CurrentBitmap.Height / dev));

                       System.IO.MemoryStream stream = new System.IO.MemoryStream();
                       currImgHandler1.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                       stream.Position = 0;
                       byte[] data = new byte[stream.Length];
                       stream.Read(data, 0, Convert.ToInt32(stream.Length));
                       BitmapImage bmapImage = new BitmapImage();
                       bmapImage.BeginInit();
                       bmapImage.StreamSource = stream;
                       bmapImage.EndInit();
                       img1.Source = bmapImage;
                   }
                   this.Height = 396.88;
                   dest.Text = "";
                  
                }
            }
            catch
            {
                
            }
        }

        private void image2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler2 = new CurrentImageHandler();
                OpenFileDialog ImageDialog = new OpenFileDialog();
                ImageDialog.RestoreDirectory = true;
                ImageDialog.FilterIndex = 1;
                ImageDialog.Filter = "bmp Files (*.bmp)|*.bmp|jpeg Files (*.jpeg)|*.jpeg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |jpg Files (*.jpg)|*.jpg|" + "All files (*.*)|*.*";


                if (ImageDialog.ShowDialog().Value)
                {
                    src2.Text = ImageDialog.FileName;
                    currImgHandler2.CurrentFileHandler.Load(ImageDialog.FileName);
                   FileInfo fileInfo2 = new FileInfo(currImgHandler2.CurrentBitmapPath);
                   if (fileInfo2.Extension == ".bmp")
                   {
                       img2.Source = new BitmapImage(new Uri(currImgHandler2.CurrentBitmapPath));
                   }
                    else
                   {
                       int dev;
                       if (currImgHandler2.CurrentBitmap.Width > currImgHandler2.CurrentBitmap.Height)
                       {
                           dev = currImgHandler2.CurrentBitmap.Width / 193;
                       }
                       else
                       {
                           dev = currImgHandler2.CurrentBitmap.Height / 193;
                       }
                       currImgHandler2.Resize(Convert.ToInt32(currImgHandler2.CurrentBitmap.Width / dev), Convert.ToInt32(currImgHandler2.CurrentBitmap.Height / dev));

                       System.IO.MemoryStream stream = new System.IO.MemoryStream();
                       currImgHandler2.CurrentBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                       stream.Position = 0;
                       byte[] data = new byte[stream.Length];
                       stream.Read(data, 0, Convert.ToInt32(stream.Length));
                       BitmapImage bmapImage = new BitmapImage();
                       bmapImage.BeginInit();
                       bmapImage.StreamSource = stream;
                       bmapImage.EndInit();
                       img2.Source = bmapImage;
                   }
                   this.Height = 396.88;
                   dest.Text = "";
                }
            }
            catch
            {
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                FileInfo fileInfo1 = new FileInfo(currImgHandler1.CurrentBitmapPath);
                FileInfo fileInfo2 = new FileInfo(currImgHandler2.CurrentBitmapPath);
                if (fileInfo1.Extension == ".bmp" || fileInfo2.Extension == ".bmp")
                {
                    Comparaison.ComparaisonClass compa = new Comparaison.ComparaisonClass();
                    dest.Text = Convert.ToString(compa.comparaison((string)currImgHandler1.CurrentBitmapPath, (string)currImgHandler2.CurrentBitmapPath))+"%";
                }
                else
                {

                ComparingImages comparing = new ComparingImages();
                dest.Text = Convert.ToString(comparing.Compare(currImgHandler1.CurrentBitmap, currImgHandler2.CurrentBitmap)) + "%";
            
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Comparaison des images", "Une erreur est survenue: " + ex.Message);

            }
            
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region animation
        private void saveEnter(object sender, MouseEventArgs e)
        {
            imgSave.Width += 5;
            imgSave.Height += 5;


        }

        private void saveLeave(object sender, MouseEventArgs e)
        {
            imgSave.Width -= 5;
            imgSave.Height -= 5;


        }

        private void openEnter(object sender, MouseEventArgs e)
        {

            imgImage.Width += 5;
            imgImage.Height += 5;
        }

        private void openLeave(object sender, MouseEventArgs e)
        {
            imgImage.Width -= 5;
            imgImage.Height -= 5;

        }
        #endregion
    }
}

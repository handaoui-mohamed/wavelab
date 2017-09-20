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
using System.ComponentModel;
using WPFFolderBrowser;

namespace Wave_Lab
{
    public partial class FenetreFiltre : MetroWindow
    {
        #region Attribut
        int k=2;
        ProgressDialogController controller;
        bool erreur;
        BackgroundWorker worker;
        string source1;
        string source2;
        #endregion

        public int type
        {
            get
            {

                return k;
            }
            set
            {
                k = value;
            }
        }

        public FenetreFiltre()
        {
            InitializeComponent();
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }    

        private void image1_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                OpenFileDialog ImageDialog = new OpenFileDialog();
                ImageDialog.RestoreDirectory = true;
                ImageDialog.FilterIndex = 1;
                ImageDialog.Filter = "jpg Files (*.jpg)|*.jpg|jpeg Files (*.jpeg)|*.jpeg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp|" + "All files (*.*)|*.*";

                if (ImageDialog.ShowDialog().Value)
                {
                    src1.Text = ImageDialog.FileName;
                    img1.Source = new BitmapImage(new Uri(ImageDialog.FileName));
                    if (img2.Source !=null || src2.Text != "")
                    {
                        img2.Source = null;
                        src2.Text = "";
                    }
                   
                    this.Height = 396.88;
                }
            }
            catch
            {

            }
        }
        
        private void image2_Click(object sender, RoutedEventArgs e)
        {

            WPFFolderBrowserDialog folder = new WPFFolderBrowserDialog();
            try
            {
                if (folder.ShowDialog() == true)
                {
                    folder.Title = "Selection Du Dossier";
                    src2.Text = folder.FileName;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Selection du dossier", "Une erreur est survenue: " + ex.Message);
            }
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            controller = await this.ShowProgressAsync("Veuillez patienter pendant l'application du filtre", "Cela peut prendre quelques secondes");
            erreur = false;
            source1 = src1.Text;
            source2 = src2.Text;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (obj, ea) => filtr();

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete);
            worker.RunWorkerAsync();
            
        }

        async void workerComplete(Object sender, RunWorkerCompletedEventArgs e)
        {
            await controller.CloseAsync();

            if (erreur)
            {
                this.ShowMessageAsync("Erreur pendant la compression !", "Veuillez verifier les informations introduites + Matlab Compiler Runtime est bien installer + votre Antivirus autorise l'application");

            }
            else
            {
                try
                {
                    img2.Source = new BitmapImage(new Uri(src2.Text));
                }
                catch
                {

                }
            }
           
        }



        private void filtr ()
        {
            try
            {
                FiltresSups.FiltresSupsClass filtre = new FiltresSups.FiltresSupsClass();
                switch (k)
                {
                    case 1:

                        filtre.laplacien((string)source1, (string)source2);
                        break;
                    case 2:
                        filtre.rehausseur((string)source1, (string)source2);
                        break;
                    case 3:
                        filtre.robert((string)source1, (string)source2);
                        break;
                    case 4:
                        filtre.sobel((string)source1, (string)source2);
                        break;
                }
            
            }
            catch
            {
                erreur = true;
            }
               
            
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using WPFFolderBrowser;
using System.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ionic.Zip;


namespace Wave_Lab
{
    public partial class Decompress : MetroWindow
    {
        #region attributs
        int s;
        public String source;
        public String source2;
        BackgroundWorker worker;
        Stopwatch sw;
        // Gestion de temps
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        ProgressDialogController controller;
        //Slection de fichier
        String[] imagesPath;
        //Methode de Transformation par Ondellet
        ListBoxItem typeItem2;
        ComboBoxItem typeItem;
        string filtre;
        string value;
        int niveau;
        bool erreur;
        #endregion

        public Decompress()
        {
            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime; 
            }
            catch (Exception e)
            {
                this.ShowMessageAsync("Changement de priorité", "Une erreur est survenue: " + e.Message);
            }

            InitializeComponent(); 
        }
        
        private void Fichier(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Title = "Selectionner Des Images";
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;
                fileDialog.FilterIndex = 1;
                fileDialog.Filter = "WaveLab Files (*.wavelab)|*.wavelab|" + "All files (*.*)|*.*";

                if (fileDialog.ShowDialog() == true)
                {
                   
                    imagesPath = new String[fileDialog.FileNames.Length];
                    Array.Copy(fileDialog.FileNames, 0, imagesPath, 0, fileDialog.FileNames.Length);
                    source = fileDialog.FileNames[0];
                    imageSrc.Text = fileDialog.FileNames[0];
                }
                
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Selection des fichiers", "Une erreur est survenue: " + ex.Message);
            }

        }

        private void Enregister_Click(object sender, RoutedEventArgs e)
        {
            WPFFolderBrowserDialog folder = new WPFFolderBrowserDialog();
            try
            {
                if (folder.ShowDialog() == true)
                {
                    folder.Title = "Selection Du Dossier";
                    source2 = folder.FileName;
                    savePath.Text = folder.FileName;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Selection du dossier", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
       
        private async void decompressionButton(object sender, RoutedEventArgs e)
        {
            controller = await this.ShowProgressAsync("Veuillez patienter pendant la decompression", "Cela peut prendre quelques minutes");
            erreur = false;
            if (dimension.SelectedIndex == 0)
            {
                s = 0;
            }
            else if (dimension.SelectedIndex == 1)
            {
                s = 1;
            }

            typeItem = (ComboBoxItem)filtres.SelectedItem;
            filtre = typeItem.Content.ToString();

            typeItem2 = (ListBoxItem)niveaux.SelectedItem;
            value = typeItem2.Content.ToString();
            niveau = Convert.ToInt32(value);
            source2 = savePath.Text;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (obj, ea) => decompress();
            
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete);
            worker.RunWorkerAsync();
        }

        async void workerComplete(Object sender, RunWorkerCompletedEventArgs e)
        {
            await controller.CloseAsync();
            if (erreur)
            {
                this.ShowMessageAsync("Erreur pendant la décompression !", "Veuillez verifier les informations introduites + Matlab Compiler Runtime est bien installer + votre Antivirus autorise l'application");
            
            }
            else
            {
                double timeInSecondsPerN = sw.Elapsed.TotalSeconds;
                String timeString = Convert.ToString(sw.Elapsed.ToString("mm\\:ss\\.ff"));
                string temp = "Temps de Decompression = " + timeString + " s";
                MessageDialogResult result = await this.ShowMessageAsync("Decompression terminer !", "" + temp + "                                          " + "Voulez vous decompresser d'autre images?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    source = "";
                    source2 = "";
                    imagesPath = null;
                    imageSrc.Text = "";
                    niveaux.SelectedIndex = -1;
                    savePath.Text = "";
                    dimension.SelectedIndex = -1;
                    filtres.SelectedIndex = -1;
                }
                else
                {
                    this.Close();
                }

            }
            
        }

        public void decompress()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";

            sw = Stopwatch.StartNew();
            
            try
            {

                if (s == 1) // folder 3D
                {
                    switch (filtre)
                    {

                        case "Filtre 2/6":
                            Decompression_2_sur_6Final.Decompression_2_sur_6FinalClass decompress2_6 = new Decompression_2_sur_6Final.Decompression_2_sur_6FinalClass();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");
                     
                                decompress2_6.decompression((string)@".\files\" + file, (string)source2);
                            }   
                            break;
                        case "Filtre 5-3":
                            Decompression_5_3Final.Decompression_5_3_FinalClass decompress5_3 = new Decompression_5_3Final.Decompression_5_3_FinalClass();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");
                     
                                decompress5_3.decompression((string)@".\files\" + file, (string)source2);
                            }   
                            break;
                        case "Filtre A(1,1)":
                            DecompressionA_1_1TR3D.DecompressionA1_1TRClass decompressa1_1 = new DecompressionA_1_1TR3D.DecompressionA1_1TRClass();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");
                     
                                decompressa1_1.decompression((string)@".\files\" + file, (string)source2);
                            }                            
                            break;
                    }
                    
                }
                else if (s == 0) // fichier 2D
                {
                    switch (filtre)
                    {
                        case "Filtre 2/6":
                            Decompression2_6.Decompression2_6Class decomp2_6 = new Decompression2_6.Decompression2_6Class();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");
                      
                                decomp2_6.decompression((string)@".\files\" + file, (string)source2);
                            }
                            break;
                        case "Filtre 5-3":
                            Decompression5_3.Decompression5_3Class decomp5_3 = new Decompression5_3.Decompression5_3Class();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");

                                decomp5_3.decompression((string)imagesPath[n], (string)source2);
                            }
                            break;
                        case "Filtre A(1,1)":
                            DeompressionA_1_1.DecompressionA_1_1Class decompa1_1 = new DeompressionA_1_1.DecompressionA_1_1Class();
                            for (int n = 0; n < imagesPath.Length; n++)
                            {                   
                                ZipFile zip = ZipFile.Read(imagesPath[n]);
                                zip.ExtractAll(".", ExtractExistingFileAction.OverwriteSilently);
                                FileInfo info = new FileInfo(imagesPath[n]);
                                string file = info.Name;
                                file = file.Replace(".wavelab", ".mat");

                                decompa1_1.decompression((string)imagesPath[n], (string)source2);
                            }
                            break;
                    }
                }
                startInfo.Arguments = @"/c RD /S /Q .\files";
                process.StartInfo = startInfo;
                process.Start();
            }
            catch (Exception ex)
            {
                erreur = true;
            }  
           sw.Stop();           
        }

        long GetDirectorySize(string p)
        {
            string[] a = Directory.GetFiles(p, "*.*");

            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }

        private void Quitter(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region animation

        private void helpEnter(object sender, MouseEventArgs e)
        {
            imgHelp.Width += 2;
            imgHelp.Height += 2;
        }

        private void helpLeave(object sender, MouseEventArgs e)
        {
            imgHelp.Width -= 2;
            imgHelp.Height -= 2;
        }

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


        private void tips_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Help_and_Tips help_tips = new Help_and_Tips();
                help_tips.ShowDialog();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void helpF1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                System.Windows.Forms.Help.ShowHelp(null, @"help.chm");
            }
        }


    }
}
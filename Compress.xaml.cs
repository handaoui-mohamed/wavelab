using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Ionic.Zip;
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


namespace Wave_Lab
{
    public partial class Compress : MetroWindow
    {
        #region Attributs
        int s;
        public String source;
        public String source2;
        long tailleE;
        long tailleS;
        BackgroundWorker worker;
        Stopwatch sw;
        // Gestion de temps
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        //Compression
        ListBoxItem typeItem2;
        ComboBoxItem typeItem;
        string filtre;
        string value;
        int niveau;
        string tauxCompression;
        ProgressDialogController controller;
        bool erreur;
        //Slection de fichier
        String[] imagesPath;
        
        #endregion


        public Compress()
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

        OpenFileDialog fileDialog = new OpenFileDialog();
        private void Fichier(object sender, RoutedEventArgs e)
        {
            try
            {
                fileDialog.Title = "Selectionner Des Images";
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;
                fileDialog.FilterIndex = 1;
                fileDialog.Filter = "bmp Files (*.bmp)|*.bmp|"+"All files (*.*)|*.*";

                if (fileDialog.ShowDialog() == true)
                {
                    imageSrc.Text = fileDialog.FileNames[0];
                }
            }
            catch 
            {
                this.ShowMessageAsync("Selection des fichiers", "Une erreur est survenue: Verifiez les informations" );
            }

        }

        private void Enregister_Click(object sender, RoutedEventArgs e)
        {

            if (dimension.SelectedIndex == 0)
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
                else if (dimension.SelectedIndex == 1)
                {
                    SaveFileDialog folder = new SaveFileDialog();
                    try
                    {
                        if (folder.ShowDialog() == true)
                        {
                            source2 = folder.FileName;
                            savePath.Text = folder.FileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowMessageAsync("Selection du dossier", "Une erreur est survenue: " + ex.Message);
                    }
                }
            else
                {
                    this.ShowMessageAsync("Erreur !", "Selectionner la dimmension avant de continuer!");
                }
            
            
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    
        private async void compressionButton(object sender, RoutedEventArgs e)
        {

            
           try
            {
                sw = Stopwatch.StartNew();
                controller = await this.ShowProgressAsync("Veuillez patienter pendant la compression", "Cela peut prendre quelques minutes");
                erreur = false;

               if (dimension.SelectedIndex == 0)
                    {
                        s = 0;
                        imagesPath = new String[fileDialog.FileNames.Length];
                        Array.Copy(fileDialog.FileNames, 0, imagesPath, 0, fileDialog.FileNames.Length);

                    }

               else 
               {
                   if (fileDialog.FileName.Length >= 4)
                   {
                       if (dimension.SelectedIndex == 1)
                       {

                           if (fileDialog.FileNames.Length % 2 == 0)
                           {

                               s = 1;
                               imagesPath = new String[fileDialog.FileNames.Length];
                               Array.Copy(fileDialog.FileNames, 0, imagesPath, 0, fileDialog.FileNames.Length);

                           }
                           else
                           {
                               s = 1;
                               imagesPath = new String[fileDialog.FileNames.Length + 1];
                               Array.Copy(fileDialog.FileNames, 0, imagesPath, 0, fileDialog.FileNames.Length);
                               imagesPath[fileDialog.FileNames.Length] = imagesPath[fileDialog.FileNames.Length - 1];

                           }
                       }
                   }
                   else
                   {
                       await this.ShowMessageAsync("Erreur d'informations !", "choisissez plus que 4 images ou plus pour la compression 3D");
                   }
               }

                


                // la taille des fichiers selectionnés
                if (imagesPath.Length > 0)
                {
                    tailleE = 0;
                    for (int n = 0; n < imagesPath.Length; n++)
                    {
                        FileInfo info = new FileInfo(imagesPath[n]);
                        tailleE += info.Length;
                    }
                }
                    
                    

                typeItem = (ComboBoxItem)filtres.SelectedItem;
                filtre = typeItem.Content.ToString();

                typeItem2 = (ListBoxItem)niveaux.SelectedItem;
                value = typeItem2.Content.ToString();
                niveau = Convert.ToInt32(value);

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += (obj, ea) => compression();

                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerComplete);
                worker.RunWorkerAsync();
            }
            catch 
            {
                this.ShowMessageAsync("Erreur d'informations !", "Veulliez verifier les information");
            }
            
           
        }

        async void workerComplete(Object sender , RunWorkerCompletedEventArgs e)
        {
            await controller.CloseAsync();
            sw.Stop(); 
            if (erreur)
            {
                this.ShowMessageAsync("Erreur pendant la compression !", "Veuillez verifier les informations introduites + Matlab Compiler Runtime est bien installer + votre Antivirus autorise l'application");

            }
            else
            {
                if (s == 1)
                {

                    tailleS = 0;
                    FileInfo info = new FileInfo(source2 +".wavelab");
                    tailleS += info.Length;

                    tauxCompression = "Taux de Compression : " + Convert.ToString(Convert.ToInt32(100 - tailleS * 100 / tailleE)) + "%";
                }
                else if (s == 0)
                {

                    tauxCompression = "Taux de Compression : " + Convert.ToString(Convert.ToInt32(100 - tailleS * 100 / tailleE)) + "%";

                }
                double timeInSecondsPerN = sw.Elapsed.TotalSeconds;
                String timeString = Convert.ToString(sw.Elapsed.ToString("mm\\:ss\\.ff"));
                string temp = "Temps de Compression = " + timeString + " s";
                MessageDialogResult result = await this.ShowMessageAsync("Compression terminer !", "" + temp + "                     " + tauxCompression + "                                          " + "Voulez vous compresser d'autre images?", MessageDialogStyle.AffirmativeAndNegative);
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

        public void compression()
        {
            
            try
            {
               

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                

                startInfo.Arguments = @"/c RD /S /Q .\tempDir";
                process.StartInfo = startInfo;
                process.Start();
                sw.Stop(); 

                startInfo.Arguments = @"/c mkdir .\tempDir";
                process.StartInfo = startInfo;
                process.Start();

                    if (s == 1) // folder 3D
                    {

                        for (int n = 0; n < imagesPath.Length; n++)
                        {
                            startInfo.FileName = "cmd.exe";
                            startInfo.Arguments = @"/c copy " + imagesPath[n] + @" .\tempDir";
                            process.StartInfo = startInfo;
                            process.Start();
                        }


                        switch (filtre)
                        {
                            case "Filtre 2/6":
                                Compression_2_sur_6Final.Compression_2_sur_6FinalClass compress2_6 = new Compression_2_sur_6Final.Compression_2_sur_6FinalClass();
                                compress2_6.compression((string)@".\tempDir", (int)niveau, (string)source2);
                                break;
                            case "Filtre 5-3":
                                Compression_5_3Final.Compression_5_3FinalClass compressa5_3 = new Compression_5_3Final.Compression_5_3FinalClass();
                                compressa5_3.compression((string)@".\tempDir", (int)niveau, (string)source2);
                                break;
                            case "Filtre A(1,1)":
                                CompressionA_1_1TR3D.CompressionA_1_1TRClass compressa1_1 = new CompressionA_1_1TR3D.CompressionA_1_1TRClass();
                                compressa1_1.compression((string)@".\tempDir", (int)niveau, (string)source2);
                                break;
                        }

                        using (ZipFile zip = new ZipFile())
                        {
                            zip.AddFile(source2 + "src.mat", "files");
                            zip.AddFile(source2 + ".mat", "files");
                            if (filtre != "Filtre A(1,1)")
                            {
                                zip.AddFile(source2 + ".dat", "files");
                            }
                            
                            zip.Name = source2 + ".wavelab";
                            zip.Save();
                        }

                        if (filtre != "Filtre A(1,1)")
                        {
                            startInfo.Arguments = @"/c DEL " + source2 + "src.mat " + source2 + ".mat " + source2 + ".dat";
                        }
                        else
                        {
                            startInfo.Arguments = @"/c DEL " + source2 + "src.mat " + source2 + ".mat " ;
                        
                        }
                        process.StartInfo = startInfo;
                        process.Start();

                    }
                    else if (s == 0) // fichier 2D
                    {
                        tailleS = 0;
                        switch (filtre)
                        {
                            case "Filtre 2/6":
                                Compression2_6TR.Compression2_6TRClass compress2_6 = new Compression2_6TR.Compression2_6TRClass();            
                                for (int n = 0; n < imagesPath.Length; n++)
                                {
                                    FileInfo info = new FileInfo(imagesPath[n]);
                                    string save = @"tempDir\" + info.Name.Replace(".bmp","");
                                    compress2_6.compression((string)imagesPath[n], (int)niveau, (string)save);
                                    
                                    using (ZipFile zip = new ZipFile())
                                    {
                                        zip.AddFile(@".\"+save + ".mat", "files");
                                        zip.Name = source2+@"\" + info.Name.Replace(".bmp", "") + ".wavelab";
                                        zip.Save();
                                    }

                                    info = new FileInfo(source2 + @"\" + info.Name.Replace(".bmp", "") + ".wavelab");
                                    tailleS += info.Length;
                                    
                                    
                                }

                                break;
                            case "Filtre 5-3":
                                Compression5_3TR.Compression5_3TRClass compress3_5 = new Compression5_3TR.Compression5_3TRClass();       
                                for (int n = 0; n < imagesPath.Length; n++)
                                {
                                    FileInfo info = new FileInfo(imagesPath[n]);
                                    string save = @"tempDir\" + info.Name.Replace(".bmp", "");
                                    compress3_5.compression((string)imagesPath[n], (int)niveau, (string)save);

                                    using (ZipFile zip = new ZipFile())
                                    {
                                        zip.AddFile(@".\" + save + ".mat", "files");
                                        zip.Name = source2 + @"\" + info.Name.Replace(".bmp", "") + ".wavelab";
                                        zip.Save();
                                    }

                                    info = new FileInfo(source2 + @"\" + info.Name.Replace(".bmp", "") + ".wavelab");
                                    tailleS += info.Length;
                                }

                                break;
                            case "Filtre A(1,1)":
                                CompressionA_1_1.CompressionA_1_1Class compressa11 = new CompressionA_1_1.CompressionA_1_1Class();                                   
                                for (int n = 0; n < imagesPath.Length; n++)
                                {
                                    FileInfo info = new FileInfo(imagesPath[n]);
                                    string save = @"tempDir\" + info.Name.Replace(".bmp", "");
                                    compressa11.compression((string)imagesPath[n], (int)niveau, (string)save);

                                    using (ZipFile zip = new ZipFile())
                                    {
                                        zip.AddFile(@".\" + save + ".mat", "files");
                                        zip.Name = source2 + @"\" + info.Name.Replace(".bmp", "") + ".wavelab";
                                        zip.Save();
                                    }

                                    info = new FileInfo(source2 + @"\" + info.Name.Replace(".bmp", "") + ".wavelab");
                                    tailleS += info.Length;
                                }

                                break;

                        }
                    }

                    startInfo.Arguments = @"/c RD /S /Q .\tempDir";
                    process.StartInfo = startInfo;
                    process.Start();

                    
            
            }
            catch (Exception ex)
            {
                erreur = true;
            }
               
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

        private void helpF1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                System.Windows.Forms.Help.ShowHelp(null, @"help.chm");
            }
        }


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


    }
}

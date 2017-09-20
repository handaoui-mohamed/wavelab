using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro;
using Microsoft.Win32;
using ImageFunctions;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Effects;

namespace Wave_Lab
{
    public partial class Image_Editor : MetroWindow
    {
        #region Decalaration
        System.Windows.Point? lastCenterPositionOnTarget;
        System.Windows.Point? lastMousePositionOnTarget;
        System.Windows.Point? lastDragPoint;

        ResizeDialog ajustDialog;

        String fullPath;

        StringBuilder apropos = new StringBuilder();

        CurrentImageHandler currImgHandler;

        ImageInfo imageInfo;
        
        Stack<Bitmap> pile_undo;
        Stack<Bitmap> pile_redo;

        private List<TabControl> tabControl;

        TabControl tab = new TabControl();

        int count = -1;

        int indexCurrent;

        #endregion


        #region Le Main de cette fenetre
        public Image_Editor()
        {
            InitializeComponent();

            apropos = apropos.Append("Encadreur de l'équipe :").Append((char)10).Append((char)10).Append("             - Kacet Sabrina ").Append((char)10).Append("             - Ayad Khadidja").Append((char)10).Append((char)10).Append((char)10);
            apropos = apropos.Append("Membre de l'équipe :").Append((char)10).Append((char)10).Append("             - Anis Fekih                    Chef d'équipe").Append((char)10).Append("             - Handaoui Mohamed").Append((char)10).Append("             - Aissaoui Ahlam").Append((char)10).Append("             - Bouchaour Houda").Append((char)10).Append("             - Heriche Walid").Append((char)10).Append("             - Laribi Hassina");

            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;

            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;

            slider.ValueChanged += OnSliderValueChanged;

            tabControl = new List<TabControl>();

            pile_undo = new Stack<Bitmap>();
            pile_redo = new Stack<Bitmap>();

            currImgHandler = new CurrentImageHandler();

        }
        #endregion


        #region Deplacement de la fenetre
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        #endregion


        #region Zoom et movement
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                System.Windows.Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }

        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y < scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }

        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(grid);

            if (e.Delta > 0)
            {
                slider.Value += 0.1;
            }
            if (e.Delta < 0)
            {
                slider.Value -= 0.1;
            }

            e.Handled = true;
        }

        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleTransform.ScaleX = e.NewValue;
            scaleTransform.ScaleY = e.NewValue;

            var centerOfViewport = new System.Windows.Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
        }

        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                System.Windows.Point? targetBefore = null;
                System.Windows.Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new System.Windows.Point(scrollViewer.ViewportWidth / 2, scrollViewer.ViewportHeight / 2);
                        System.Windows.Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(grid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / grid.Width;
                    double multiplicatorY = e.ExtentHeight / grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }
        #endregion


        #region Evenement des Buttons 

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"Aide\WebHelp\index.html");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, @"Aide\help.chm");
        }

        private void contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Contact contacting = new Contact();
                contacting.ShowDialog();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Chargement de la fenetre", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void resize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ajustDialog = new ResizeDialog();
                ajustDialog.NewWidth = currImgHandler.CurrentBitmap.Width;
                ajustDialog.NewHeight = currImgHandler.CurrentBitmap.Height;
                if (ajustDialog.ShowDialog().Value)
                {
                    currImgHandler.Resize(ajustDialog.NewWidth, ajustDialog.NewHeight);
                    information.Text = imageInfo.ImageInformation(currImgHandler);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Ajustement de l'image","Une erreur est survenue: " + ex.Message);
            }
        }

        private void about_Click(object sender, RoutedEventArgs e)
        {

            this.ShowMessageAsync("A propos de nous", apropos.ToString());
        }


        private void fichier_Compresser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Compress isWnd = new Compress();
                isWnd.ShowDialog();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Compression des images", "Une erreur est survenue: " + ex.Message);
            }
        }

        // la fenetre de selection d'image

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

        private void fichier_ouvrir_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ImageDialog = new OpenFileDialog();
            ImageDialog.RestoreDirectory = false;
            ImageDialog.FilterIndex = 1;
            ImageDialog.Filter = "jpg Files (*.jpg)|*.jpg|jpeg Files (*.jpeg)|*.jpeg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp|"+"All files (*.*)|*.*";

            try
            {
                if (ImageDialog.ShowDialog().Value)
                {

                    fullPath = ImageDialog.FileName;
                    currImgHandler.CurrentFileHandler.Load(ImageDialog.FileName);
                    imageInfo = new ImageInfo();
                    information.Text = imageInfo.ImageInformation(currImgHandler);
                    tabControl.Add(new TabControl(currImgHandler.CurrentBitmap, pile_undo, pile_redo,information.Text));
                    pile_redo.Clear();
                    pile_undo.Clear();
                    pile_undo.Push(currImgHandler.CurrentBitmap);        
                    PaintImage();
                    

                    FileInfo fileInfo = new FileInfo(currImgHandler.CurrentBitmapPath);

                    TabItem newTab = new TabItem();
                    newTab.Header = fileInfo.Name.Replace(fileInfo.Extension, "");
                    newTab.MouseDoubleClick += new MouseButtonEventHandler(tab_MouseDoubleClick);
                    tabDynamic.Items.Add(newTab);
                    if (count == -1) count = 0;
                    indexCurrent = count;
                    tabDynamic.SelectedIndex = count++;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Chargement de l'image", "Une erreur est survenue: " + ex.Message);
                
            }



        }

        // fenetre pour enregister  une image
        private void fichier_Enregistrer_sous_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.RestoreDirectory = true;
            saveDialog.FilterIndex = 1;
            saveDialog.Filter = "jpg Files (*.jpg)|*.jpg|jpeg Files (*.jpeg)|*.jpeg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp|" + "All files (*.*)|*.*";
            try
            {
                if (saveDialog.ShowDialog().Value)
                {
                    CurrentImageHandler temp = new CurrentImageHandler();
                    temp.CurrentBitmap = currImgHandler.CurrentBitmap;
                    currImgHandler.CurrentFileHandler.Save(saveDialog.FileName);
                    currImgHandler.CurrentBitmap = temp.CurrentBitmap;
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Sauvgarde de l'image", "Une erreur est survenue: " + ex.Message);
            }

        }

        private void edition_undo_Click(object sender, RoutedEventArgs e)
        {
            
            if (pile_undo.Count > 1)
            {
                pile_redo.Push(pile_undo.Pop());
                currImgHandler.CurrentBitmap = pile_undo.Peek();
                information.Text = imageInfo.ImageInformation(currImgHandler);
                PaintImage();
            }  
        }

        private void edition_redo_Click(object sender, RoutedEventArgs e)
        {
            if (pile_redo.Count > 0)
            {
                pile_undo.Push(pile_redo.Pop());
                currImgHandler.CurrentBitmap = pile_undo.Peek();
                information.Text = imageInfo.ImageInformation(currImgHandler);
                PaintImage();
            }
        }

        private void image_grayScal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentGrayscaleHandler.SetGrayscale();
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Niveaux de Gris de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }
        

        private void image_180_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentRotationHandler.Flip(RotateFlipType.Rotate180FlipNone);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Inversement de l'image", "Une erreur est survenue: " + ex.Message);
               
            }
        }

        private void image_90h_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentRotationHandler.Flip(RotateFlipType.Rotate90FlipNone);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Inversement de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void image_90a_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentRotationHandler.Flip(RotateFlipType.Rotate270FlipNone);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Inversement de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void image_hori_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentRotationHandler.Flip(RotateFlipType.RotateNoneFlipX);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Inversement de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void image_vert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentRotationHandler.Flip(RotateFlipType.RotateNoneFlipY);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Inversement de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void ui_language_Click(object sender, RoutedEventArgs e)
        {


        }

        private void zoom_ar_Click(object sender, RoutedEventArgs e)
        {
            slider.Value--;
        }

        private void zoom_av_Click(object sender, RoutedEventArgs e)
        {
            slider.Value++;
        }

        private void quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Impression de l'imgae actuelle
        private void print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            try
            {
                if (printDialog.ShowDialog().Value)
                {
                    printDialog.PrintVisual(this.image, "Impression");
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Impression de l'image", "Une erreur est survenue: " + ex.Message);
                
            }
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ShowMessageAsync("Information", imageInfo.ImageInformation(currImgHandler));
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Information de l'image", "Une erreur est survenue: " + ex.Message);

            }
            
        }

        private void filtreRouge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentFilterHandler.SetColorFilter(ColorFilterTypes.Red);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Couleur de l'image", "Une erreur est survenue: " + ex.Message);
                
            }
        }

        private void filtreBleu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentFilterHandler.SetColorFilter(ColorFilterTypes.Blue);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Couleur de l'image", "Une erreur est survenue: " + ex.Message);
                
            }
        }

        private void filtreVert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentFilterHandler.SetColorFilter(ColorFilterTypes.Green);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Couleur de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void tab_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TabItem tab = sender as TabItem;


                if (MessageBox.Show(string.Format("Etes-vous sûr de supprimer l'onglet '{0}'?", tab.Header.ToString()),
                        "supprimer l'onglet", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    tabControl.Remove(tabControl[indexCurrent]);
                    currImgHandler.ClearImage();
                    image.Source = null;
                    tabDynamic.Items.Remove(tabDynamic.SelectedItem as TabItem);
                    count--;
                }
            }
            catch
            {

            }
            
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //currImgHandler.CurrentBitmap, pile_undo, pile_redo,information.Text
            //MessageBox.Show(Convert.ToString(indexCurrent), "test");
                if (tabControl.Count != 0)
                {
                    tabControl[indexCurrent].setBitmap(currImgHandler.CurrentBitmap);
                    tabControl[indexCurrent].setPile_undo(pile_undo);
                    tabControl[indexCurrent].setPile_redo(pile_redo);
                    tabControl[indexCurrent].setInformation(information.Text);
                    //////////
                    tab = tabControl[tabDynamic.SelectedIndex];
                    indexCurrent = tabDynamic.SelectedIndex;
                    pile_redo = tab.getPile_redo();
                    pile_undo = tab.getPile_undo();
                    information.Text = tab.getInformation();
                    currImgHandler.CurrentBitmap = tab.getBitmap();
                    PaintImage();
                }
            }
            catch
            {

            }
            
            
            
        }

        private void BitmapViewMenu_Click(object sender, RoutedEventArgs e)
        {
            BitmapViewWindow bvWnd = new BitmapViewWindow(currImgHandler);
            bvWnd.Show();
        }

        public Bitmap Generate_negative_image(Bitmap sourceimage)
        {
            Color c;
            for (int i = 0; i < sourceimage.Width; i++)
            {
                for (int j = 0; j < sourceimage.Height; j++)
                {
                    c = sourceimage.GetPixel(i, j);
                    c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                    sourceimage.SetPixel(i, j, c);
                }
            }

            return sourceimage;
        }

        private void inverse_Click(object sender, RoutedEventArgs e)
        {
            // probleme de undo
            currImgHandler.CurrentBitmap = Generate_negative_image(currImgHandler.CurrentBitmap);
            pile_undo.Push(currImgHandler.CurrentBitmap);
            pile_redo.Clear();
            PaintImage();
        }

        private void SepiaToneMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currImgHandler.CurrentSepiaToneHandler.SetSepiaTone();
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Effet de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void BrightnessMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BrightnessWindow bWnd = new BrightnessWindow();
                bWnd.BrightnessValue = 0;
                CurrentImageHandler img = new CurrentImageHandler();
                img.CurrentBitmap = currImgHandler.CurrentBitmap;
                int dev;
                if (img.CurrentBitmap.Width > img.CurrentBitmap.Height)
                {
                    dev = img.CurrentBitmap.Width / 229;
                }
                else
                {
                    dev = img.CurrentBitmap.Height / 229;
                }
                img.Resize(Convert.ToInt32(img.CurrentBitmap.Width / dev),Convert.ToInt32(img.CurrentBitmap.Height / dev));
                bWnd.bitmap = img.CurrentBitmap;
                if (bWnd.ShowDialog().Value)
                {
                    currImgHandler.CurrentBrightnessHandler.SetBrightness(bWnd.BrightnessValue);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Effet de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void ConstrastMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ContrastWindow cFrm = new ContrastWindow();
                cFrm.ContrastValue = 0;
                CurrentImageHandler img = new CurrentImageHandler();
                img.CurrentBitmap = currImgHandler.CurrentBitmap;
                int dev;
                if (img.CurrentBitmap.Width > img.CurrentBitmap.Height)
                {
                    dev = img.CurrentBitmap.Width / 229;
                }
                else
                {
                    dev = img.CurrentBitmap.Height / 229;
                }
                img.Resize(Convert.ToInt32(img.CurrentBitmap.Width / dev), Convert.ToInt32(img.CurrentBitmap.Height / dev));
                cFrm.bitmap = img.CurrentBitmap;
                if (cFrm.ShowDialog().Value)
                {
                    currImgHandler.CurrentContrastHandler.SetContrast(cFrm.ContrastValue);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Effet de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void crop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            Rognage crop = new Rognage();
            crop.currentImage = currImgHandler.CurrentBitmap;
            if (crop.ShowDialog() == true)
            {
                currImgHandler.CurrentBitmap = crop.currentImage;
                information.Text = imageInfo.ImageInformation(currImgHandler);
                pile_undo.Push(currImgHandler.CurrentBitmap);
                pile_redo.Clear();
                PaintImage();
            }

             }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Rognage de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void TextInsertMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InsertTextWindow itWnd = new InsertTextWindow();
                itWnd.DisplayTextPositionX = itWnd.DisplayTextPositionY = 0;
                if (itWnd.ShowDialog().Value)
                {
                    currImgHandler.CurrentTextInsHandler.Insert(itWnd.DisplayText, itWnd.DisplayTextPositionX, itWnd.DisplayTextPositionY, itWnd.DisplayTextFont, itWnd.DisplayTextFontSize, itWnd.DisplayTextFontStyle, itWnd.DisplayTextAngle, itWnd.DisplayTextOpacity, itWnd.DisplayTextColor1, itWnd.DisplayTextColor2, itWnd.DisplayTextGradientStyle);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void ImageInsertMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InsertImageWindow iiWnd = new InsertImageWindow();
                iiWnd.DisplayImagePositionX = iiWnd.DisplayImagePositionY = 0;
                if (iiWnd.ShowDialog().Value)
                {
                    currImgHandler.CurrentImgInsHandler.Insert(iiWnd.DisplayImagePath, iiWnd.DisplayImagePositionX, iiWnd.DisplayImagePositionY, iiWnd.DisplayImageWidth, iiWnd.DisplayImageHeight, iiWnd.DisplayImageAngle, iiWnd.DisplayImageOpacity);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void ShapeInsertMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InsertShapeWindow isWnd = new InsertShapeWindow();
                isWnd.DisplayShapePositionX = isWnd.DisplayShapePositionY = 0;
                if (isWnd.ShowDialog().Value)
                {
                    currImgHandler.CurrentShapeInsHandler.Insert(isWnd.DisplayShape, isWnd.DisplayShapePositionX, isWnd.DisplayShapePositionY, isWnd.DisplayShapeWidth, isWnd.DisplayShapeHeight, isWnd.DisplayShapeAngle, isWnd.DisplayShapeOpacity, isWnd.DisplayShapeColor1, isWnd.DisplayShapeColor2, isWnd.DisplayShapeGradientStyle);
                    pile_undo.Push(currImgHandler.CurrentBitmap);
                    pile_redo.Clear();
                    PaintImage();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void compare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    Compare isWnd = new Compare();
                    isWnd.ShowDialog();
               
                
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Comparaison des images", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void helpF1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                System.Windows.Forms.Help.ShowHelp(null, @"help.chm");
            }
        }


       /* int k;
        private void laplacien_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                k = 1;
                FenetreFiltre filtre = new FenetreFiltre();
                filtre.type = k;

                filtre.ShowDialog();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }

        }

        private void rehausseur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                k = 2;
                FenetreFiltre filtre = new FenetreFiltre();
                filtre.type = k;
                filtre.ShowDialog();
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void robert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                k = 3;
                FenetreFiltre filtre = new FenetreFiltre();
                filtre.type = k;
                if (filtre.ShowDialog().Value)
                {

                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        private void sobel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                k = 4;
                FenetreFiltre filtre = new FenetreFiltre();
                filtre.type = k;
                if (filtre.ShowDialog().Value)
                {

                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }*/

        private void fichier_Decompression_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                Decompress decompression = new Decompress();

                if (decompression.ShowDialog().Value)
                {

                }
            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
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

        private void waveLab_about_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Prez_Credit prez_Credit = new Prez_Credit();
                prez_Credit.ShowDialog();

            }
            catch (Exception ex)
            {
                this.ShowMessageAsync("Insertion de l'image", "Une erreur est survenue: " + ex.Message);
            }
        }

        #endregion


        #region Changement du Theme

        private void rouge_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("Red"),
                                        theme.Item1);
        }

        private void vert_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

           ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent("Green"),
                                        theme.Item1);
           
        }

        private void bleu_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Blue"),
                                         theme.Item1);
             }

        private void pourpre_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Purple"),
           theme.Item1);
        }

        private void orange_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Orange"),
                                         theme.Item1);
           
        }

        private void citron_vert_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Lime"),
                                         theme.Item1);
            
        }

        private void emeraude_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Emerald"),
                                         theme.Item1);
            
        }

        private void sarcellle_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Teal"),
                                         theme.Item1);
        }

        private void cyan_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Cyan"),
                                         theme.Item1);

        }

        private void cobalt_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Cobalt"),
                                         theme.Item1);

        }

        private void indigo_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Indigo"),
                                         theme.Item1);

        }

        private void violet_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Violet"),
                                         theme.Item1);
        }

        private void rose_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Pink"),
                                         theme.Item1);
        }

        private void magenta_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Magenta"),
                                         theme.Item1);

        }

        private void cramoisi_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Crimson"),
                                         theme.Item1);
        }

        private void ambre_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Amber"),
                                         theme.Item1);
        }

        private void jaune_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Yellow"),
                                         theme.Item1);


        }

        private void brun_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Brown"),
                                         theme.Item1);
 
        }

        private void olive_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Olive"),
                                         theme.Item1);
        }

        private void acier_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Steel"),
                                         theme.Item1);
        }

        private void mauve_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Mauve"),
                                         theme.Item1);
        }

        private void taupe_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Taupe"),
                                         theme.Item1);
        }

        private void sienna_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppStyle(Application.Current,
                                         ThemeManager.GetAccent("Sienna"),
                                         theme.Item1);
        }

        private void dark_Click(object sender, RoutedEventArgs e)
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            ThemeManager.ChangeAppTheme(Application.Current, "BaseDark");
            information.Foreground = System.Windows.Media.Brushes.White;
            blanc.Visibility = Visibility.Hidden;
            blanc2.Visibility = Visibility.Hidden;
            cropButton.Foreground = System.Windows.Media.Brushes.White;
            resizeButton.Foreground = System.Windows.Media.Brushes.White;
            inserImageButton.Foreground = System.Windows.Media.Brushes.White;
            inserTextButton.Foreground = System.Windows.Media.Brushes.White;
            inserShapeButton.Foreground = System.Windows.Media.Brushes.White;
            contrastButton.Foreground = System.Windows.Media.Brushes.White;
            lumButton.Foreground = System.Windows.Media.Brushes.White;
            inverseButton.Foreground = System.Windows.Media.Brushes.White;
            sepiaButton.Foreground = System.Windows.Media.Brushes.White;
            grayButton.Foreground = System.Windows.Media.Brushes.White;
            filter1.Opacity = 0.5;
            filter2.Opacity = 0.5;
            
        }     

        private void light_Click(object sender, RoutedEventArgs e)
        {
            blanc.Visibility = Visibility.Visible;
            blanc2.Visibility = Visibility.Visible;
            information.Foreground = System.Windows.Media.Brushes.Black;
            cropButton.Foreground = System.Windows.Media.Brushes.Black;
            resizeButton.Foreground = System.Windows.Media.Brushes.Black;
            inserImageButton.Foreground = System.Windows.Media.Brushes.Black;
            inserTextButton.Foreground = System.Windows.Media.Brushes.Black;
            inserShapeButton.Foreground = System.Windows.Media.Brushes.Black;
            contrastButton.Foreground = System.Windows.Media.Brushes.Black;
            lumButton.Foreground = System.Windows.Media.Brushes.Black;
            inverseButton.Foreground = System.Windows.Media.Brushes.Black;
            sepiaButton.Foreground = System.Windows.Media.Brushes.Black;
            grayButton.Foreground = System.Windows.Media.Brushes.Black;
            filter1.Opacity = 0.4;
            filter2.Opacity = 0.4;
            

        }
        #endregion    


        #region quelque animations de buttons
        private void openEnter(object sender, MouseEventArgs e)
        {
            imgOpen.Width += 2;
            imgOpen.Height += 2;
        }

        private void openLeave(object sender, MouseEventArgs e)
        {
            imgOpen.Width -= 2;
            imgOpen.Height -= 2;
        }

        private void printEnter(object sender, MouseEventArgs e)
        {
            imgPrint.Width += 2;
            imgPrint.Height += 2;
        }

        private void printLeave(object sender, MouseEventArgs e)
        {
            imgPrint.Width -= 2;
            imgPrint.Height -= 2;
        }

        private void saveEnter(object sender, MouseEventArgs e)
        {
            imgSave.Width += 2;
            imgSave.Height += 2;
        }

        private void saveLeave(object sender, MouseEventArgs e)
        {
            imgSave.Width -= 2;
            imgSave.Height -= 2;

        }

        private void annulerEnter(object sender, MouseEventArgs e)
        {

            imgAnnuler.Width += 2;
            imgAnnuler.Height += 2;
        }

        private void annulerLeave(object sender, MouseEventArgs e)
        {

            imgAnnuler.Width -= 2;
            imgAnnuler.Height -= 2;
        }

        private void redoEnter(object sender, MouseEventArgs e)
        {
            imgRedo.Width += 2;
            imgRedo.Height += 2;

        }

        private void redoLeave(object sender, MouseEventArgs e)
        {
            imgRedo.Width -= 2;
            imgRedo.Height -= 2;
        }

         private void r90hEnter(object sender, MouseEventArgs e)
        {
            img90h.Width += 2;
            img90h.Height += 2;
            text90h.FontSize +=1;
        }

        private void r90hLeave(object sender, MouseEventArgs e)
        {
            img90h.Width -= 2;
            img90h.Height -= 2;
            text90h.FontSize -= 1;
        }

        private void r90aEnter(object sender, MouseEventArgs e)
        {
            img90a.Width += 2;
            img90a.Height += 2;
            text90a.FontSize += 1;
        }

        private void r90aLeave(object sender, MouseEventArgs e)
        {
            img90a.Width -= 2;
            img90a.Height -= 2;
            text90a.FontSize -= 1;
        }

        private void mhEnter(object sender, MouseEventArgs e)
        {
            imgMh.Width += 2;
            imgMh.Height += 2;
        }

        private void mhLeave(object sender, MouseEventArgs e)
        {
            imgMh.Width -= 2;
            imgMh.Height -= 2;

        }

        private void mvEnter(object sender, MouseEventArgs e)
        {
            imgMv.Width += 2;
            imgMv.Height += 2;
        }

        private void mvLeave(object sender, MouseEventArgs e)
        {
            imgMv.Width -= 2;
            imgMv.Height -= 2;

        }

        private void zoomavEnter(object sender, MouseEventArgs e)
        {
            imgZoomav.Width += 2;
            imgZoomav.Height += 2;
        }

        private void zoomavLeave(object sender, MouseEventArgs e)
        {
            imgZoomav.Width -= 2;
            imgZoomav.Height -= 2;
        }

        private void zoomarEnter(object sender, MouseEventArgs e)
        {
            imgZoomar.Width += 2;
            imgZoomar.Height += 2;
        }

        private void zoomarLeave(object sender, MouseEventArgs e)
        {
            imgZoomar.Width -= 2;
            imgZoomar.Height -= 2;
        }

        private void fullEnter(object sender, MouseEventArgs e)
        {
            imgFull.Width += 2;
            imgFull.Height += 2;
        }

        private void fullLeave(object sender, MouseEventArgs e)
        {
            imgFull.Width -= 2;
            imgFull.Height -= 2;
        }

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

        private void aboutEnter(object sender, MouseEventArgs e)
        {
            imgAbout.Width += 2;
            imgAbout.Height += 2;
        }

        private void aboutLeave(object sender, MouseEventArgs e)
        {
            imgAbout.Width -= 2;
            imgAbout.Height -= 2;
        }

        #endregion

        
    }
}
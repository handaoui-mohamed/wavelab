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
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Threading;

namespace Wave_Lab
{
    public partial class SelectionWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTime = new System.Windows.Threading.DispatcherTimer();
        Storyboard storySpin ;

        public SelectionWindow()
        {
            Thread.Sleep(3000);
            InitializeComponent();

            storySpin = (Storyboard)this.image.FindResource("spin");
            
            dispatcherTime.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTime.Interval = TimeSpan.FromMilliseconds(2000);

            dispatcherTime.Start();  
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!r1.IsMouseOver && !r2.IsMouseOver && 
                !r3.IsMouseOver && !r4.IsMouseOver &&
                !compressButton.IsMouseOver &&
                !decompressButton.IsMouseOver &&
                !editorButton.IsMouseOver &&
                !exitButton.IsMouseOver &&
                !imgCenter.IsMouseOver)
            {
                Storyboard sb = this.FindResource("animR1_end") as Storyboard;
                Storyboard.SetTarget(sb, r1);
                sb.Begin();
                Storyboard.SetTarget(sb, decompressButton);
                sb.Begin();
                r1.Effect = null;

                sb = this.FindResource("animR2_end") as Storyboard;
                Storyboard.SetTarget(sb, r2);
                sb.Begin();
                Storyboard.SetTarget(sb, exitButton);
                sb.Begin();
                r2.Effect = null;

                sb = this.FindResource("animR3_end") as Storyboard;
                Storyboard.SetTarget(sb, r3);
                sb.Begin();
                Storyboard.SetTarget(sb, editorButton);
                sb.Begin();
                r3.Effect = null;

                sb = this.FindResource("animR4_end") as Storyboard;
                Storyboard.SetTarget(sb, r4);
                sb.Begin();
                Storyboard.SetTarget(sb, compressButton);
                sb.Begin();
                r4.Effect = null;

                imgCenter.Effect = null;

                
            }
        }
        
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                Compress compress= new Compress();
                this.Visibility = Visibility.Hidden;
                compress.ShowDialog();
                this.Visibility = Visibility.Visible;
            }
            catch
            {
                
            }
        }

        private void Tile_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {

                Decompress decompress = new Decompress();
                this.Visibility = Visibility.Hidden;
                decompress.ShowDialog();
                this.Visibility = Visibility.Visible;
            }
            catch 
            {
                
            }
        }

        private void Tile_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {

                Image_Editor image_Editor = new Image_Editor();
                this.Visibility = Visibility.Hidden;
                image_Editor.ShowDialog();
                this.Visibility = Visibility.Visible;
                
            }
            catch
            {
               
            }
        }

        
        private void animation(object sender, MouseEventArgs e)
        {
            storySpin.Stop();
                
                r4.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 10,
                        Opacity = 100
                    };
                r1.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 10,
                        Opacity = 100
                    };
                r2.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 10,
                        Opacity = 100
                    };
                r3.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 10,
                        Opacity = 100
                    };

                imgCenter.Effect =
                    new DropShadowEffect
                    {
                        Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                        Direction = 320,
                        ShadowDepth = 0,
                        BlurRadius = 10,
                        Opacity = 100
                    };

                Storyboard sb = this.FindResource("animR1") as Storyboard;
                Storyboard.SetTarget(sb, r1);
                sb.Begin();
                Storyboard.SetTarget(sb, decompressButton);
                sb.Begin();

               

                sb = this.FindResource("animR2") as Storyboard;
                Storyboard.SetTarget(sb, r2);
                sb.Begin();
                Storyboard.SetTarget(sb, exitButton);
                sb.Begin();
                    

                sb = this.FindResource("animR3") as Storyboard;
                Storyboard.SetTarget(sb, r3);
                sb.Begin();
                Storyboard.SetTarget(sb, editorButton);
                sb.Begin();
                
                
                sb = this.FindResource("animR4") as Storyboard;
                Storyboard.SetTarget(sb, r4);
                sb.Begin();
                Storyboard.SetTarget(sb, compressButton);
                sb.Begin();


                


            

        }
        
        private void Storyboard_Completed(object sender, EventArgs e)
        
        {
            Storyboard sb;
            if (imgCenter.IsMouseOver)
            {
                                
                storySpin.Begin();
                storySpin.SetSpeedRatio(27);
                imgCenter.Effect =
                new DropShadowEffect
                {
                   
                    Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 50,
                    Opacity = 100
                };
            }

            
            if (r1.IsMouseOver || decompressButton.IsMouseOver)
            {
                sb = this.FindResource("animR1_after") as Storyboard;
                Storyboard.SetTarget(sb, r1);
                sb.Begin();
                Storyboard.SetTarget(sb, decompressButton);
                sb.Begin();
                r1.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 50,
                    Opacity = 100
                };
            }
            if (r2.IsMouseOver || exitButton.IsMouseOver)
            {
                sb = this.FindResource("animR2_after") as Storyboard;
                Storyboard.SetTarget(sb, r2);
                sb.Begin();
                Storyboard.SetTarget(sb, exitButton);
                sb.Begin();
                r2.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 50,
                    Opacity = 100
                };
            }
            if (r3.IsMouseOver || editorButton.IsMouseOver)
            {
                sb = this.FindResource("animR3_after") as Storyboard;
                Storyboard.SetTarget(sb, r3);
                sb.Begin();
                Storyboard.SetTarget(sb, editorButton);
                sb.Begin();
                r3.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 50,
                    Opacity = 100
                };
            }
            if (r4.IsMouseOver || compressButton.IsMouseOver)
            {
                sb = this.FindResource("animR4_after") as Storyboard;
                Storyboard.SetTarget(sb, r4);
                sb.Begin();
                Storyboard.SetTarget(sb, compressButton);
                sb.Begin();
                r4.Effect =
                new DropShadowEffect
                {
                    Color = new Color { A = 100, R = 255, G = 255, B = 255 },
                    Direction = 320,
                    ShadowDepth = 0,
                    BlurRadius = 50,
                    Opacity = 100
                };
            }
        }

        private void Storyboard_Completed_1(object sender, EventArgs e)
        {
            
        }



        private DateTime downTime;
        private object downSender;
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released &&
                sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    Prez_Credit credit = new Prez_Credit();
                    this.Visibility = Visibility.Hidden;
                    credit.ShowDialog();
                    this.Visibility = Visibility.Visible;
                }
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

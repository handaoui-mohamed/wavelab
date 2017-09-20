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
using System.Windows.Media.Effects;


namespace Wave_Lab
{

    public partial class Prez_Credit : MetroWindow
    {
        #region Data
        // Used when manually scrolling.
        private Point scrollStartPoint;
        private Point scrollStartOffset;
        System.Windows.Threading.DispatcherTimer dispatcherTime = new System.Windows.Threading.DispatcherTimer();
        int i = 0;
        #endregion
        
        public  Prez_Credit()
        {
            InitializeComponent();
            dispatcherTime.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTime.Interval = TimeSpan.FromMilliseconds(10);
            
            dispatcherTime.Start();  
        }
        

        #region Mouse Events
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (ScrollViewer.IsMouseOver)
            {

                // Save starting point, used later when determining how much to scroll.
                scrollStartPoint = e.GetPosition(this);
                scrollStartOffset.X = ScrollViewer.HorizontalOffset;
                scrollStartOffset.Y = ScrollViewer.VerticalOffset;

                // Update the cursor if can scroll or not.
                this.Cursor = (ScrollViewer.ExtentWidth > ScrollViewer.ViewportWidth) ||
                    (ScrollViewer.ExtentHeight > ScrollViewer.ViewportHeight) ?
                    Cursors.Pen : Cursors.Pen;

                this.CaptureMouse();
            }

            base.OnPreviewMouseDown(e);
        }


        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                dispatcherTime.Stop();
                space.Margin = new Thickness(0, 0, 0, 0); 
                // Get the new scroll position.
                Point point = e.GetPosition(this);

                // Determine the new amount to scroll.
                Point delta = new Point(
                    (point.X > this.scrollStartPoint.X) ?
                        -(point.X - this.scrollStartPoint.X) :
                        (this.scrollStartPoint.X - point.X),

                    (point.Y > this.scrollStartPoint.Y) ?
                        -(point.Y - this.scrollStartPoint.Y) :
                        (this.scrollStartPoint.Y - point.Y));

                // Scroll to the new position.
                ScrollViewer.ScrollToHorizontalOffset(this.scrollStartOffset.X + delta.X);
                ScrollViewer.ScrollToVerticalOffset(this.scrollStartOffset.Y + delta.Y);
            }

            base.OnPreviewMouseMove(e);
        }



        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                this.Cursor = Cursors.Pen;
                this.ReleaseMouseCapture();
            }

            base.OnPreviewMouseUp(e);
        }
        #endregion



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
           
            if (i != 2000)
            {
                ScrollViewer.ScrollToVerticalOffset(this.scrollStartOffset.X + i);
                i++;
            }
            else
            {
                space.Margin = new Thickness(0, 0, 0, 0); 
                dispatcherTime.Stop();
            }
            
        }


    }
}

using System;
using System.Windows;
using Microsoft.Win32;
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Windows.Media;
using System.Windows.Input;

namespace Wave_Lab
{

    public partial class InsertImageWindow : MetroWindow
    {
        OpenFileDialog oDlg;
        public InsertImageWindow()
        {
            InitializeComponent();
            

            oDlg = new OpenFileDialog(); 
            oDlg.RestoreDirectory = true;
            oDlg.InitialDirectory = "C:\\";
            oDlg.FilterIndex = 1;
            oDlg.Filter = "jpg Files (*.jpg)|*.jpg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp";
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public string DisplayImagePath
        {
            get { return txtImage.Text; }
            set { txtImage.Text = value.ToString(); }
        }

        public int DisplayImagePositionX
        {
            get
            {
                if (string.IsNullOrEmpty(txtX.Text))
                    txtX.Text = "0";
                return Convert.ToInt32(txtX.Text);
            }
            set { txtX.Text = value.ToString(); }
        }

        public int DisplayImagePositionY
        {
            get
            {
                if (string.IsNullOrEmpty(txtY.Text))
                    txtY.Text = "0";
                return Convert.ToInt32(txtY.Text);
            }
            set { txtY.Text = value.ToString(); }
        }

        public int DisplayImageWidth
        {
            get { return Convert.ToInt32(txtWidth.Text); }
            set { txtWidth.Text = value.ToString(); }
        }

        public int DisplayImageHeight
        {
            get { return Convert.ToInt32(txtHeight.Text); }
            set { txtHeight.Text = value.ToString(); }
        }

        public float DisplayImageAngle
        {
            get
            {
                if (string.IsNullOrEmpty(txtAngle.Text))
                    txtAngle.Text = "0";
                return Convert.ToSingle(txtAngle.Text);
            }
            set { txtAngle.Text = value.ToString(); }
        }

        public int DisplayImageOpacity
        {
            get { return Convert.ToInt32(txtOpacity.Text); }
            set { txtOpacity.Text = value.ToString(); }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            if (oDlg.ShowDialog().Value)
            {
                txtImage.Text = oDlg.FileName;
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

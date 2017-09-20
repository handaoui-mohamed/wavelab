using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Windows.Media;
using System.Windows.Input;

namespace Wave_Lab
{
    public partial class InsertShapeWindow : MetroWindow
    {
        public InsertShapeWindow()
        {
            InitializeComponent();
            
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public string DisplayShape
        {
            get { return cmbShapes.Text; }
            set { cmbShapes.Text = value.ToString(); }
        }

        public int DisplayShapePositionX
        {
            get
            {
                if (string.IsNullOrEmpty(txtX.Text))
                    txtX.Text = "0";
                return Convert.ToInt32(txtX.Text);
            }
            set { txtX.Text = value.ToString(); }
        }

        public int DisplayShapePositionY
        {
            get
            {
                if (string.IsNullOrEmpty(txtY.Text))
                    txtY.Text = "0";
                return Convert.ToInt32(txtY.Text);
            }
            set { txtY.Text = value.ToString(); }
        }

        public int DisplayShapeWidth
        {
            get
            {
                if (string.IsNullOrEmpty(txtWidth.Text))
                    txtWidth.Text = "0";
                return Convert.ToInt32(txtWidth.Text);
            }
            set { txtWidth.Text = value.ToString(); }
        }

        public int DisplayShapeHeight
        {
            get
            {
                if (string.IsNullOrEmpty(txtHeight.Text))
                    txtHeight.Text = "0";
                return Convert.ToInt32(txtHeight.Text);
            }
            set { txtHeight.Text = value.ToString(); }
        }

        public float DisplayShapeAngle
        {
            get
            {
                if (string.IsNullOrEmpty(txtAngle.Text))
                    txtAngle.Text = "0";
                return Convert.ToSingle(txtAngle.Text);
            }
            set { txtAngle.Text = value.ToString(); }
        }

        public int DisplayShapeOpacity
        {
            get { return Convert.ToInt32(txtOpacity.Text); }
            set { txtOpacity.Text = value.ToString(); }
        }

        public string DisplayShapeColor1
        {
            get { return cmbColors1.Text; }
            set { cmbColors1.Text = value.ToString(); }
        }

        public string DisplayShapeColor2
        {
            get { return cmbColors2.Text; }
            set { cmbColors2.Text = value.ToString(); }
        }

        public string DisplayShapeGradientStyle
        {
            get { return cmbGradientStyle.Text; }
            set { cmbGradientStyle.Text = value.ToString(); }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load Shapes.
            cmbShapes.Items.Add("Ellipse");
            cmbShapes.Items.Add("Rectangle");
            cmbShapes.Items.Add("FilledEllipse");
            cmbShapes.Items.Add("FilledRectangle");
            // Load Colors.
            Type type = typeof(System.Drawing.Color);
            System.Reflection.PropertyInfo[] propertyInfo = type.GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                if (propertyInfo[i].Name != "Transparent"
                    && propertyInfo[i].Name != "R"
                    && propertyInfo[i].Name != "G"
                    && propertyInfo[i].Name != "B"
                    && propertyInfo[i].Name != "A"
                    && propertyInfo[i].Name != "IsKnownColor"
                    && propertyInfo[i].Name != "IsEmpty"
                    && propertyInfo[i].Name != "IsNamedColor"
                    && propertyInfo[i].Name != "IsSystemColor"
                    && propertyInfo[i].Name != "Name")
                {
                    cmbColors1.Items.Add(propertyInfo[i].Name);
                    cmbColors2.Items.Add(propertyInfo[i].Name);
                }
            }
            // Load Gradient Styles
            cmbGradientStyle.Items.Add("Horizontal");
            cmbGradientStyle.Items.Add("Vertical");
            cmbGradientStyle.Items.Add("BackwardDiagonal");
            cmbGradientStyle.Items.Add("ForwardDiagonal");
        }

        private void chkGradient_Checked(object sender, RoutedEventArgs e)
        {
            grpGradient.IsEnabled = true;
        }

        private void chkGradient_Unchecked(object sender, RoutedEventArgs e)
        {
            grpGradient.IsEnabled = false;
        }
    }
}

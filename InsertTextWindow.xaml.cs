using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro;
using System.Windows.Input;

namespace Wave_Lab
{
    public partial class InsertTextWindow : MetroWindow
    {
        public InsertTextWindow()
        {
            InitializeComponent();
            
        }

        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public string DisplayText
        {
            get { return txtText.Text; }
            set { txtText.Text = value.ToString(); }
        }

        public int DisplayTextPositionX
        {
            get
            {
                if (string.IsNullOrEmpty(txtX.Text))
                    txtX.Text = "0";
                return Convert.ToInt32(txtX.Text);
            }
            set { txtX.Text = value.ToString(); }
        }

        public int DisplayTextPositionY
        {
            get
            {
                if (string.IsNullOrEmpty(txtY.Text))
                    txtY.Text = "0";
                return Convert.ToInt32(txtY.Text);
            }
            set { txtY.Text = value.ToString(); }
        }

        public string DisplayTextFont
        {
            get { return cmbFonts.Text; }
            set { cmbFonts.Text = value.ToString(); }
        }

        public float DisplayTextFontSize
        {
            get
            {
                float fs = 10.0F;
                if (!string.IsNullOrEmpty(cmbFontSize.Text))
                    fs = Convert.ToSingle(cmbFontSize.Text.Replace("pt", ""));
                return fs;
            }
            set { cmbFonts.Text = value.ToString() + "pt"; }
        }

        public string DisplayTextFontStyle
        {
            get { return cmbFontStyles.Text; }
            set { cmbFontStyles.Text = value.ToString(); }
        }

        public float DisplayTextAngle
        {
            get
            {
                if (string.IsNullOrEmpty(txtAngle.Text))
                    txtAngle.Text = "0";
                return Convert.ToSingle(txtAngle.Text);
            }
            set { txtAngle.Text = value.ToString(); }
        }

        public int DisplayTextOpacity
        {
            get { return Convert.ToInt32(txtOpacity.Text); }
            set { txtOpacity.Text = value.ToString(); }
        }

        public string DisplayTextColor1
        {
            get { return cmbColors1.Text; }
            set { cmbColors1.Text = value.ToString(); }
        }

        public string DisplayTextColor2
        {
            get { return cmbColors2.Text; }
            set { cmbColors2.Text = value.ToString(); }
        }

        public string DisplayTextGradientStyle
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
            // Load Fonts.
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies.OrderBy(f => f.Source))
            {
                cmbFonts.Items.Add(fontFamily.Source);
            }

            // Load Font Size.
            for (int i = 5; i <= 75; i += 5)
            {
                cmbFontSize.Items.Add(i.ToString() + "pt");
            }
            // Load Font Styles.
            cmbFontStyles.Items.Add("Bold");
            cmbFontStyles.Items.Add("Italic");
            cmbFontStyles.Items.Add("Regular");
            cmbFontStyles.Items.Add("Strikeout");
            cmbFontStyles.Items.Add("Underline");
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

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkdownAuthoring
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public string Title { get; set; }
        public Color ResultColor=> ColorComboBox!=null?(Color)ColorComboBox.SelectedItem:Colors.Blue;
        public string ColorName=> ColorComboBox.SelectedValue.ToString();
        public ColorPicker()
        {
            InitializeComponent();
            LoadColors();
            ColorComboBox.SelectedItem = "Blue";
        }
        private void LoadColors()
        {
            var colorNames = typeof(Colors).GetProperties()
                                            .Select(p => p.Name)
                                            .OrderBy(name => name);

            foreach (var colorName in colorNames)
            {
                ColorComboBox.Items.Add(colorName);
            }
        }

        private void ColorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
            if (ColorComboBox.SelectedItem != null)
            {
                string selectedColorName = ColorComboBox.SelectedItem.ToString();
                var color = (Color)ColorConverter.ConvertFromString(selectedColorName);
                ColorPreview.Background = new SolidColorBrush(color);
            }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ColorPicker), new PropertyMetadata(""));
        public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register("ColorName", typeof(string), typeof(ColorPicker), new PropertyMetadata(""));
    }
}

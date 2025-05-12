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
    /// Interaction logic for ListStyle.xaml
    /// </summary>
    public partial class ListStyle : UserControl
    {
        public string Title { get; set; }
        public string ResultStyle => StyleBox.SelectedItem != null ? StyleBox.SelectedValue.ToString() : "Disc";
        public event EventHandler ChangeStyle;
        public ListStyle()
        {
            InitializeComponent();
            loadStylers();
        }
        private void loadStylers()
        {
            string[] styles = ["Disc","Square","Circle"];

            foreach (var style in styles)
            {
                StyleBox.Items.Add(style);
            }
            StyleBox.SelectedItem= styles[0];
        }
        private void StyleBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChangeStyle != null)
            {
                ChangeStyle(this, new EventArgs());
            }
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ListStyle), new PropertyMetadata(""));
    }
}

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

namespace Milestone1App
{
    /// <summary>
    /// Interaction logic for PopUpBox.xaml
    /// </summary>
    public partial class PopUpBox : Window
    {
        public string Category { get { return CategoryBox.Text; } }

        public PopUpBox()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.Width;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

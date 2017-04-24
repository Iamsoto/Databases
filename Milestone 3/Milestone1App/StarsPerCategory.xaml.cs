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
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;

namespace Milestone1App
{
    /// <summary>
    /// Interaction logic for StarsPerCategory.xaml
    /// </summary>
    public partial class StarsPerCategory : Window
    {
        Dictionary<string, double> starsPerCategory = new Dictionary<string, double>();

        public StarsPerCategory()
        {
            InitializeComponent();
        }

        public void load()
        {
            ((ColumnSeries)StarsPerCategoryChart.Series[0]).ItemsSource = starsPerCategory.ToArray();
        }

        public void AddCategory(string category, double stars)
        {
            starsPerCategory[category] = stars;
        }
    }
}

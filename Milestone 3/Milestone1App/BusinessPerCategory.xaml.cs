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
using Npgsql;
using System.Windows.Controls.DataVisualization;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.DataVisualization.Charting;

namespace Milestone1App
{
    /// <summary>
    /// Interaction logic for BusinessPerCategory.xaml
    /// </summary>
    public partial class BusinessPerCategory : Window
    {
        List<string> categories;
        string connection_string;
        public BusinessPerCategory(List<string> categories, string connection_string)
        {
            InitializeComponent();
            this.categories = categories;
            this.connection_string = connection_string;
            load();
        }

        // Returns count of query per category
        public int getCount(string query)
        {
            int count = 0;

            // Create connection
            using (var conn = new NpgsqlConnection(connection_string))
            {
                try
                {
                    conn.Open();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                // Execute sql command
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = query;

                    // Add to statelist
                    using (var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        if(reader.HasRows)
                        {
                            count = reader.GetInt32(0);
                        }
                          
                    }
                }
                conn.Close();
            }
            return count;
        }

        // ... 
        public void load()
        {

            Dictionary<string, int> business_categories = new Dictionary<string, int>();
            foreach (string category in categories)
            {
                int i = 0;
                string query =
                    @"SELECT COUNT (c.business_id) 
                      FROM categories as c  
                      WHERE category = '" + category + "' GROUP BY c.category";
                int count = getCount(query);
                business_categories[category] = count;
                i++;
                
            }
            ((ColumnSeries)BusinessPerCategoryChart.Series[0]).ItemsSource = business_categories.ToArray();
        }

    }
}

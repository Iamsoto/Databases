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
    /// Interaction logic for CheckinChart.xaml
    /// </summary>
    public partial class CheckinChart : Window
    {
        string _business_id, _conn;
        public CheckinChart(string business_id, string conn)
        {
            this._business_id = business_id;
            this._conn = conn;
            InitializeComponent();
            findNumWeekdays();
        }


        public List<int> executeQuery(string query, int num_attr = 1)
        {
            List<int> items = new List<int>();
            // Create connection
            using (var conn = new NpgsqlConnection(_conn))
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
                        while (reader.Read())
                        {
                           items.Add(reader.GetInt32(0));                           
                        }
                            
                    }
                }
                conn.Close();
            }
            return items;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void ProgressBar_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        // ...
        public void findNumWeekdays()
        {
            double[] num_checkins_weekdays = new double[7];
            int total = 0;
            for (int i = 0; i < 7; i++)
            {
                string query_String = "SELECT count FROM checkin WHERE business_id = '" + _business_id + "' AND weekday_num = " + i +";";
                List<int> counts = executeQuery(query_String);
                foreach( int count in counts )
                {
                    num_checkins_weekdays[i] += (double)count;
                    total += count;
                }


                ((ColumnSeries)CheckinChartGrid.Series[0]).ItemsSource = new KeyValuePair<string, int>[] 
                {
                    new KeyValuePair<string, int>("Monday", (int)num_checkins_weekdays[1]),
                    new KeyValuePair<string, int>("Tuesday", (int)num_checkins_weekdays[2]),
                    new KeyValuePair<string, int>("Wednesday", (int)num_checkins_weekdays[3]),
                    new KeyValuePair<string, int>("Thursday", (int)num_checkins_weekdays[4]),
                    new KeyValuePair<string, int>("Friday", (int)num_checkins_weekdays[5]),
                    new KeyValuePair<string, int>("Saturday", (int)num_checkins_weekdays[6]),
                    new KeyValuePair<string, int>("Sunday", (int)num_checkins_weekdays[0]),

                };
            }

            



        }
    }
}

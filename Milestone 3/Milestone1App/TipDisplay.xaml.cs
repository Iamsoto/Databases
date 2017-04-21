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

namespace Milestone1App
{
    /// <summary>
    /// Interaction logic for TipDisplay.xaml
    /// </summary>
    public partial class TipDisplay : Window
    {
        public class TipInfo
        {
            public string user_name { get; set; }
            public string tip_text { get; set; }
            public string date { get; set; }
            public int num_likes { get; set; }

        }

        string connection;
        string business_id;
        public TipDisplay(string business_id, string connection)
        {
            this.connection = connection;
            this.business_id = business_id;
            InitializeComponent();
            addTipsColumns();
            runQuery();
        }

        // Bind the data grid
        public void addTipsColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("user_name");
            col1.Width = 75;
            dataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Date";
            col2.Binding = new Binding("date");
            dataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "number of likes";
            col3.Binding = new Binding("num_likes");
            dataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Tip";
            col4.Binding = new Binding("tip_text");
            dataGrid.Columns.Add(col4);

        }

        //Execute the query
        public void runQuery()
        {
            // Create connection
            using (var conn = new NpgsqlConnection(connection))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText =
                    @"SELECT user_name, yelp_text, yelp_date, likes FROM tips t
                    INNER JOIN yelp_user u ON u.user_id = t.user_id
                    INNER JOIN business b ON b.business_id = t.business_id
                    WHERE t.business_id = 'J3INwTy4KZJ8aJwVoOqAJA'";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipInfo info = new TipInfo();
                            if (reader.HasRows)
                            {
                                info.user_name = reader.GetString(0);
                                info.tip_text = reader.GetString(1);
                                info.date = reader.GetString(2);
                                info.num_likes = reader.GetInt32(3);
                                dataGrid.Items.Add(info);
                            }

                        }
                    }
                }


                conn.Close();
            }


        }
    }
}

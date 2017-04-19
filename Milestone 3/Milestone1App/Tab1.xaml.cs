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
using Npgsql;

namespace Milestone1App
{

    // CustomTipInfo
    public class TipInfo
    {
        public string user_name { get; set; }
        public string business_name { get; set; }
        public string city { get; set; }
        public string tip_text { get; set; }
        public string date { get; set; }

    }
    /*
     * User Tab 
     */
    public partial class Tab1 : UserControl
    {
        // Tab 1 initialization
        public Tab1()
        {
            InitializeComponent();
            addTipsColumns();
        }

        /*
        */
        public void addTipsColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("user_name");
            col1.Width = 75;
            dataGrid.Columns.Add(col1);
            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("business_id");
            dataGrid.Columns.Add(col2);
            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            dataGrid.Columns.Add(col3);
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Tip";
            col4.Binding = new Binding("tip_text");
            dataGrid.Columns.Add(col4);


        }


        // Builds a connection string
        private string buildConnString()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone2";
        }



        // Use_ID textbox change
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Create connection
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }

                // Execute sql command, Obtain User's friends
                List<string> friend_ids = new List<string>();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id FROM friend WHERE friend_of = '" + textBox.Text + "';";
                    using (var readerFriendIds = cmd.ExecuteReader())
                    {
                        // For each friend create a new list of friend_ids and list of tip infos 

                        // Find the friends ID's 
                        while (readerFriendIds.Read())
                        {
                            friend_ids.Add(readerFriendIds.GetString(0));
                        }
                    }
                
                List<TipInfo> tipInfos = new List<TipInfo>();
                    // For each friend_Id
                    foreach (string friend_user_id in friend_ids)
                    {
                        string user_command =
                           @"SELECT user_name, b.business_name, b.city, t.yelp_text, t.yelp_date 
                        FROM yelp_user
                        INNER JOIN tips t ON t.user_id = yelp_user.user_id
                        LEFT JOIN  business b ON t.business_id = b.business_id
                        WHERE yelp_user.user_id = '" + friend_user_id + "'; ";

                        // Get tipInfo
                        cmd.CommandText = user_command;
                        using (var readerTipInfo = cmd.ExecuteReader())
                        {
                            // Add result to dataGrid
                            readerTipInfo.Read();
                            if(readerTipInfo.HasRows)
                            {
                                dataGrid.Items.Add(new TipInfo()
                                {
                                    user_name = readerTipInfo.GetString(0),
                                    business_name = readerTipInfo.GetString(1),
                                    city = readerTipInfo.GetString(2),
                                    tip_text = readerTipInfo.GetString(3),
                                    date = readerTipInfo.GetString(4),
                                });
                            }

                        }
                    }
                }

                conn.Close();
            }

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }

}






    


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

    public class Friend
    {
        public string user_name { get; set; }
        public double average_stars { get; set; }
        public string yelping_since { get; set; }
        public string user_id { get; set; }
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
            addFriendInfo();
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
            col2.Binding = new Binding("business_name");
            dataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            dataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Tip";
            col4.Binding = new Binding("tip_text");
            dataGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Date";
            col5.Binding = new Binding("date");
            col5.SortDirection =  System.ComponentModel.ListSortDirection.Descending;
            dataGrid.Columns.Add(col5);

        }

        /**
         * Manages friend grid 1
         */ 
        public void addFriendInfo()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("user_name");
            FriendGrid1.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "avg_star";
            col2.Binding = new Binding("average_stars");
            FriendGrid1.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "yelping_since";
            col3.Binding = new Binding("yelping_since");
            FriendGrid1.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "ID";
            col4.Binding = new Binding("user_id");
            FriendGrid1.Columns.Add(col4);

        }

        // Builds a connection string
        private string buildConnString()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone2";
        }



        // Use_ID textbox change
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }
        

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        // When the user is set
        private void button_Click(object sender, RoutedEventArgs e)
        {
            // No id has length less than 5!
            if (textBox.Text.Length < 5)
            {
                return;
            }

            // Flush out any old values
            dataGrid.Items.Clear();
            FriendGrid1.Items.Clear();

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

                    //Find the user's info
                    cmd.CommandText =
                        "SELECT user_name, average_stars, fans, yelping_since "
                        + "FROM yelp_user "
                        + "WHERE user_id = '" + textBox.Text + "';";
                    using (var readerUser = cmd.ExecuteReader())
                    {
                        readerUser.Read();
                        textBox1.Text = textBox2.Text = textBox3.Text  = "";
                        label5.Content = "";
                        if (readerUser.HasRows)
                        {
                            // Name textbox
                            textBox1.Text = readerUser.GetString(0);

                            // Average Stars
                            textBox2.Text = readerUser.GetDouble(1).ToString();

                            // Fans
                            textBox3.Text = readerUser.GetDouble(2).ToString();

                            //Yelping since laebl
                            label5.Content = readerUser.GetString(3);
                        }

                    }

                    //Find the user's Vote info
                    cmd.CommandText = @"SELECT c.vote_count, u.vote_count, f.vote_count
                                        FROM vote c
                                        INNER JOIN vote u ON u.user_id = c.user_id
                                        INNER JOIN vote f ON f.user_id = u.user_id
                                        WHERE 
                                        c.vote_attribute = 'cool' AND u.vote_attribute= 'useful' 
                                        AND f.vote_attribute = 'funny' AND
                                        c.user_id = '" + textBox.Text + "'";

                    using (var readerVotes = cmd.ExecuteReader())
                    {
                        readerVotes.Read();
                        textBox4.Text = textBox5.Text = textBox6.Text = " ";
                        if (readerVotes.HasRows)
                        {
                            // Cool Votes
                            textBox4.Text = readerVotes.GetInt64(0).ToString();
                            //Useful votes
                            textBox6.Text = readerVotes.GetInt64(1).ToString();
                            //Funny votes
                            textBox5.Text = readerVotes.GetInt64(2).ToString();

                        }

                    }


                            // Find the user's friends
                            cmd.CommandText = "SELECT user_id FROM friend WHERE friend_of = '" + textBox.Text + "';";
                    using (var readerFriendIds = cmd.ExecuteReader())
                    {
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

                        // Get tipInfo for friend
                        cmd.CommandText = user_command;
                        using (var readerTipInfo = cmd.ExecuteReader())
                        {
                            // Add result to dataGrid
                            readerTipInfo.Read();
                            if (readerTipInfo.HasRows)
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

                        // Execute friend query
                        cmd.CommandText = @"SELECT user_name, average_stars, yelping_since FROM yelp_user 
                                           WHERE user_id = '" + friend_user_id + "';";
                        using (var readerFriend = cmd.ExecuteReader())
                        {
                            readerFriend.Read();
                            if (readerFriend.HasRows)
                            {
                                // Add item to friend Grid
                                FriendGrid1.Items.Add(new Friend
                                {
                                    user_name = readerFriend.GetString(0),
                                    average_stars = readerFriend.GetDouble(1),
                                    yelping_since = readerFriend.GetString(2)
                                });
                            }
                        }
                    }
                }
                conn.Close();
            }
            dataGrid.ColumnFromDisplayIndex(4).SortDirection = System.ComponentModel.ListSortDirection.Descending;
        }


        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Nothing
        }
    }

}






    


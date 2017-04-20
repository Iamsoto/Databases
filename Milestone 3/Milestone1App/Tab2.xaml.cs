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

/*<Label Grid.Column="0" Grid.Row="0" Content="State" HorizontalAlignment="Left" Margin="10,10,0,0" VerticalAlignment="Top"/>
            <ComboBox Grid.Column="0" Grid.Row="0" Name="StateCombobox" HorizontalAlignment="Left" Margin="52,14,0,0" VerticalAlignment="Top" Width="120" SelectionChanged="StateCombobox_SelectionChanged"/>
            <Label Grid.Column="0" Grid.Row="1" Content="City" HorizontalAlignment="Left" Margin="10,41,0,0" VerticalAlignment="Top"/>
            <ListBox Grid.Column="0" Grid.Row="2" Name="CitiesListBox" HorizontalAlignment="Stretch" Margin="10,72,0,0" VerticalAlignment="Stretch" SelectionChanged="CitiesListBox_SelectionChanged"/>
            <Label Grid.Column="0" Grid.Row="3" Content="Zipcode" HorizontalAlignment="Left" Margin="10,177,0,0" VerticalAlignment="Top" Width="160"/>
            <ListBox Grid.Column="0" Grid.Row="4" Name="ZipCodeList" HorizontalAlignment="Left" Height="250" Margin="10,208,0,0" VerticalAlignment="Top" Width="160"/>
            <Button Grid.Column="0" Grid.Row="5" Content="Add" HorizontalAlignment="Left" Margin="10,463,0,0" VerticalAlignment="Top" Width="75" Click="Button_Click"/>
            <Button Grid.Column="0" Grid.Row="5" Name="RemoveButton" Content="Remove" HorizontalAlignment="Left" Margin="97,463,0,0" VerticalAlignment="Top" Width="75" Click="RemoveButton_Click"/>
            <ListBox Grid.Column="0" Grid.Row="6"  Name="CategoryListBox" Margin="12,488,566,107" Width="160"/>
            <Button Grid.Column="0" Grid.Row="7" Content="Search Businesses" HorizontalAlignment="Left" Margin="10,618,0,0" VerticalAlignment="Top" Width="162"/>
            <Label Content="Day of the week" HorizontalAlignment="Left" Margin="200,10,0,0" VerticalAlignment="Top"/>
            <ComboBox HorizontalAlignment="Left" Margin="300,14,0,0" VerticalAlignment="Top" Width="120"/>
            <Label Content="From" HorizontalAlignment="Left" Margin="425,10,0,0" VerticalAlignment="Top"/>
            <ComboBox HorizontalAlignment="Left" Margin="467,14,0,0" VerticalAlignment="Top" Width="120"/>
            <ComboBox HorizontalAlignment="Left" Margin="619,14,-1,0" VerticalAlignment="Top" Width="120"/>
            <Label Content="To" HorizontalAlignment="Left" Margin="592,10,0,0" VerticalAlignment="Top"/>*/

namespace Milestone1App
{
    public class BusinessInfo
    {
        public string business_id { get; set; }
        public string business_name { get; set; }
        public string full_address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string stars { get; set; }
        public string yelp_type { get; set; }
        public string open { get; set; }
        public string review_count { get; set; }
        public string num_checkins { get; set; }
    }

    /// <summary>
    /// Interaction logic for Tab2.xaml
    /// </summary>
    public partial class Tab2 : UserControl
    {
        public Tab2()
        {
            InitializeComponent();
            addStates();
            addDaysOfTheWeek();
            addHours();
            addBusinessColumns();
            defaultHour();
            defaultState();
        }

        void defaultState()
        {
            StateCombobox.SelectedItem = StateCombobox.Items[0];
        }

        void defaultHour()
        {
            WeekdayBox.SelectedItem = WeekdayBox.Items[0];
            FromBox.SelectedItem = FromBox.Items[0];
            ToBox.SelectedItem = ToBox.Items[ToBox.Items.Count - 1];
        }

        void addBusinessColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Id";
            col1.Binding = new Binding("business_id");

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business Name";
            col2.Binding = new Binding("business_name");

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Full Address";
            col3.Binding = new Binding("full_address");

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "City";
            col5.Binding = new Binding("city");

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Latitude";
            col6.Binding = new Binding("latitude");

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Longitude";
            col7.Binding = new Binding("longitude");

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Stars";
            col8.Binding = new Binding("stars");

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Yelp Type";
            col9.Binding = new Binding("yelp_type");

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Header = "Open";
            col10.Binding = new Binding("open");

            DataGridTextColumn col11 = new DataGridTextColumn();
            col11.Header = "Review Count";
            col11.Binding = new Binding("review_count");

            DataGridTextColumn col12 = new DataGridTextColumn();
            col12.Header = "Number of Checkins";
            col12.Binding = new Binding("num_checkins");

            BusinessGrid.Columns.Add(col1);
            BusinessGrid.Columns.Add(col2);
            BusinessGrid.Columns.Add(col3);
            BusinessGrid.Columns.Add(col4);
            BusinessGrid.Columns.Add(col5);
            BusinessGrid.Columns.Add(col6);
            BusinessGrid.Columns.Add(col7);
            BusinessGrid.Columns.Add(col8);
            BusinessGrid.Columns.Add(col9);
            BusinessGrid.Columns.Add(col10);
            BusinessGrid.Columns.Add(col11);
            BusinessGrid.Columns.Add(col12);
        }

        void addHours()
        {
            for (int i = 0; i < 24; i++)
            {
                string hour = ((i < 10) ? "0" + i.ToString() : i.ToString()) + ":00";
                FromBox.Items.Add(hour);
                ToBox.Items.Add(hour);
            }
        }

        void addDaysOfTheWeek()
        {
            string[] weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            foreach(string day in weekdays){
                WeekdayBox.Items.Add(day);
            }
        }

        private string buildConnString()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone3";
        }

        public List<string> executeQuery(string query) 
        {
            List<string> items = new List<string>();
            // Create connection
            using (var conn = new NpgsqlConnection(buildConnString()))
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
                            items.Add(reader.GetString(0));
                    }
                }
                conn.Close();
            }
            return items;
        }

        public void addCities(string state)
        {
            try
            {
                foreach (string city in executeQuery("SELECT DISTINCT city FROM business WHERE state='" + state + "'"))
                {
                    CitiesListBox.Items.Add(city);
                }
            }
            catch
            {
                CitiesListBox.Items.Add("Error");
            }
        }

        public void addStates()
        {
            foreach (string item in executeQuery("SELECT DISTINCT state FROM business;")){
                StateCombobox.Items.Add(item);
            }
        }

        private void StateCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < CitiesListBox.Items.Count; i++)
                CitiesListBox.Items.RemoveAt(i);
            addCities((string)StateCombobox.SelectedValue);
        }

        private void addZips()
        {
            string query = "SELECT DISTINCT full_address FROM business WHERE state='" +
                (string)StateCombobox.SelectedValue + "'" + " AND " + "city='" + (string)CitiesListBox.SelectedItem + "'";
            foreach (string item in executeQuery(query))
            {
                string[] items = item.Split(' ');
                string zip_code = items.Last();
                ZipCodeList.Items.Add(zip_code);
            }
        }

        private void CitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < ZipCodeList.Items.Count; i++)
            {
                ZipCodeList.Items.RemoveAt(i);
            }
            addZips();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PopUpBox p = new PopUpBox();
            p.Closed += popup_Closed;
            p.Show();
        }

        void popup_Closed(object sender, EventArgs e)
        {
            CategoryListBox.Items.Add((sender as PopUpBox).Category);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryListBox.Items.RemoveAt(CategoryListBox.SelectedIndex);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string query = "SELECT * FROM business WHERE ";
            query += "state='" + StateCombobox.SelectedItem + "'";
            query += " AND ";
            query += "city='" + CitiesListBox.SelectedItem + "'";
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                try{ conn.Open(); }
                catch { }

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
                            var this_business = new BusinessInfo()
                            {
                                business_id = reader.GetString(0),
                                business_name = reader.GetString(1),
                                full_address = reader.GetString(2),
                                state = reader.GetString(3),
                                city = reader.GetString(4),
                                latitude = reader.GetDouble(5).ToString(),
                                longitude = reader.GetDouble(6).ToString(),
                                stars = reader.GetDouble(7).ToString(),
                                yelp_type = reader.GetString(8),
                                open = reader.GetString(9),
                                review_count = reader.GetInt32(10).ToString(),
                                num_checkins = reader.GetInt32(11).ToString(),
                            };
                            BusinessGrid.Items.Add(this_business);
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}

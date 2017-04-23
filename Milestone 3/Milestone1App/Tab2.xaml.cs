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
using System.Threading;

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
            populateCategories();
            addBusinessColumns();
            defaultHour();
            defaultState();
            defaultCity();
            defaultZip();
            defaultSelectedCategories();
        }

        void defaultSelectedCategories()
        {
            SelectedCategoryListBox.Items.Add("Restaurants");
            SelectedCategoryListBox.Items.Add("Sandwiches");
        }

        void defaultZip()
        {
            ZipCodeList.SelectedItem = ZipCodeList.Items[ZipCodeList.Items.IndexOf("85003")];
        }

        void defaultCity()
        {
            CitiesListBox.SelectedItem = CitiesListBox.Items[CitiesListBox.Items.IndexOf("Phoenix")];
        }

        void populateCategories()
        {
            List<string> categories = new List<string>();
            foreach (string category in executeQuery("SELECT DISTINCT category FROM categories"))
            {
                categories.Add(category);
            }
            categories.Sort();
            foreach (string category in categories)
            {
                CategoriesListBox.Items.Add(category);
            }
        }

        void defaultState()
        {
            StateCombobox.SelectedItem = StateCombobox.Items[StateCombobox.Items.IndexOf("AZ")];
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
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone2";
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
                List<string> cities = new List<string>();
                foreach (string city in executeQuery("SELECT DISTINCT city FROM business WHERE state='" + state + "'"))
                {
                    cities.Add(city);
                }
                cities.Sort();
                foreach (string city in cities)
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
            while (CitiesListBox.Items.Count > 0)
            {
                CitiesListBox.Items.RemoveAt(0);
            }
            addCities((string)StateCombobox.SelectedValue);
        }

        private void addZips()
        {
            string query = "SELECT DISTINCT full_address FROM business WHERE state='" +
                (string)StateCombobox.SelectedValue + "'" + " AND " + "city='" + (string)CitiesListBox.SelectedItem + "'";
            Dictionary<string, string> all_zips = new Dictionary<string, string>();
            foreach (string item in executeQuery(query))
            {
                string[] items = item.Split(' ');
                string zip_code = items.Last();
                int zip;
                if (!ZipCodeList.Items.Contains(zip_code) && int.TryParse(zip_code, out zip))
                {
                   all_zips[zip_code.ToString()] = (zip_code.ToString());
                }
            }
            List<string> zips = new List<string>(all_zips.Values);
            zips.Sort();
            foreach (string zip in zips)
            {
                ZipCodeList.Items.Add(zip);
            }
        }

        private void CitiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            while (ZipCodeList.Items.Count > 0)
            {
                ZipCodeList.Items.RemoveAt(0);
            }
            addZips();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cell in CategoriesListBox.SelectedItems)
            {
                SelectedCategoryListBox.Items.Add(cell);
            }
        }

        void popup_Closed(object sender, EventArgs e)
        {
            //CategoryListBox.Items.Add((sender as PopUpBox).Category);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedCategoryListBox.Items.RemoveAt(SelectedCategoryListBox.SelectedIndex);
        }

        private string AND()
        {
            return " AND ";
        }

        string searchBusinessesQuery(string day, string start_time, string end_time, string category, string zip)
        {
            string query = string.Format("SELECT B.business_id\nFROM business AS B {0} \nWHERE B.business_id = C.business_id\n", ", categories as C");
            query += AND();
            query += "B.state = '" + StateCombobox.SelectedItem + "'" + "\n";
            query += AND();
            query += "B.city='" + CitiesListBox.SelectedItem + "'" + "\n";
            if (category != ""){
                query += AND();
                query += "C.category = '" + category + "'";
            }
            query += AND();
            query += " B.full_address LIKE '%" + zip + "'";
            return query;
        }

        private void clearBusinessDataGrid()
        {
            while (BusinessGrid.Items.Count > 0)
            {
                BusinessGrid.Items.RemoveAt(0);
            }
        }

        /*
        SELECT *
        FROM business AS B, week_days AS W
        WHERE B.business_id = W.business_id
        AND B.state = 'AZ'
        AND B.city = 'Wickenburg' 
        AND W.open_hour >= '11:00'
        AND W.close_hour <= '23:00'
        */

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Dictionary<string, BusinessInfo> businesses = new Dictionary<string, BusinessInfo>();

            clearBusinessDataGrid();

            string week = (string)WeekdayBox.SelectedValue;
            string start = (string)FromBox.SelectedValue;
            string end = (string)ToBox.SelectedValue;

            string query = SelectedCategoryListBox.Items.Count == 0 ? searchBusinessesQuery(week, start, end, "", (string)ZipCodeList.SelectedItem) : "";

            string operation = "\n INTERSECT \n";

            foreach (string category in SelectedCategoryListBox.Items)
            {
                query += "\n" + searchBusinessesQuery(week, start, end, category, (string)ZipCodeList.SelectedItem) + "\n";
                query += operation;
            }

            if (SelectedCategoryListBox.Items.Count > 0)
            {
                query = query.Remove(query.Count() - operation.Length);
            }

            //query = "SELECT DISTINCT business_id FROM (\n" + query + "\n) AS B, business as B1\nWHERE B.business_id = B1.business_id\n";

            query = "SELECT DISTINCT B.business_id, business_name, full_address, state, city, latitude, longitude, stars, yelp_type, open, review_count, num_checkins FROM (\n" + query + "\n) AS S, Business as B, week_days as W\n";
            query = query + "WHERE S.business_id = B.business_id AND W.business_id = B.business_id AND W.open_hour>='" + FromBox.SelectedItem + "'" + " AND W.close_hour<='" + ToBox.SelectedItem + "'" + " AND W.weekday='" + WeekdayBox.SelectedItem + "'";

            Clipboard.SetText(query);

            Thread messageThread = new Thread(() => MessageBox.Show(query));
            messageThread.Start();

            using (var conn = new NpgsqlConnection(buildConnString())) {
                try { conn.Open(); }
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
                                business_id = reader.GetString(0), business_name = reader.GetString(1), full_address = reader.GetString(2),
                                state = reader.GetString(3), city = reader.GetString(4), latitude = reader.GetDouble(5).ToString(),
                                longitude = reader.GetDouble(6).ToString(), stars = reader.GetDouble(7).ToString(), yelp_type = reader.GetString(8),
                                open = reader.GetString(9), review_count = reader.GetInt32(10).ToString(), num_checkins = reader.GetInt32(11).ToString(),
                            };
                            businesses[this_business.business_id] = this_business;
                        }
                    }
                }
            }
            Thread fetchedThread = new Thread(() => MessageBox.Show(string.Format("Fetched {0} items", businesses.Count)));
            fetchedThread.Start();
            foreach (var business in businesses){
                BusinessGrid.Items.Add(business.Value);
            }
        }

        BusinessInfo selected_business = null;

        private void BusinessGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (SelectedBusinessBox.Items.Count > 0)
                SelectedBusinessBox.Items.RemoveAt(0);
            selected_business = (BusinessInfo)BusinessGrid.SelectedValue;

            if(selected_business == null)
            {
                return;
            }

            Clipboard.SetText(selected_business.business_id);
            SelectedBusinessBox.Items.Add(selected_business.business_name);
        }

        int checkin_num = 0;

        private void CheckinButton_Click(object sender, RoutedEventArgs e)
        {
            //INSERT INTO checkin VALUES ('6UGAtEw8TH6J6YeRRZpwKg', 5, 3, 2, 'checkin') 
            //create a new checkin
            string business_id = selected_business.business_id;
            int hour_start = FromBox.SelectedIndex; //convert the string hour start to a number
            int weekday_num = WeekdayBox.SelectedIndex; //convert weekday string to a number
            int count = ToBox.SelectedIndex - hour_start;
            string query = string.Format("INSERT INTO CHECKIN VALUES ('{0}', {1}, {2}, {3}, '{4}')", business_id, hour_start, weekday_num, count, "checkin" + checkin_num.ToString());
            executeQuery(query);
        }

        public Func<Friend> selected_friend;


        private void AddTipButton_Click(object sender, RoutedEventArgs e)
        {
            string user_id = "5RhNs6Vkd1iq07-lyCKV6Q"; //FIGURE OUT WTF THIS IS SUPPOSED TO BE
            string business_id = selected_business.business_id;
            string tip_text = TipTextBox.Text; //the text of the tip
            string likes = "0";
            DateTime current_date = DateTime.Now;
            string date = (string.Format("{0:yy/MM/dd}", current_date).ToString());

            List<string> num_tips = executeQuery("SELECT COUNT(*) FROM tips");
            int tip_num = int.Parse(num_tips[0]);

            string yelp_type = "";
            string query = string.Format("INSERT INTO tips VALUES ({0}, '{1}', '{2}', '{3}', {4}, '{5}', '{6}')", tip_num, user_id, business_id, tip_text, likes, date, yelp_type);
            MessageBox.Show(query);
            Clipboard.SetText(query);
            executeQuery(query);
            //ZlvVCqFKTMK4rBkTQzVBpA
        }





        // Upon button clicks
        private void ShowCheckinsButton_Click(object sender, RoutedEventArgs e)
        {
            if(selected_business == null)
            {
                return;
            }
            CheckinChart chart = new CheckinChart(selected_business.business_id, buildConnString());
             chart.Show();
            

        }

        private void ShowTipsButton_Click(object sender, RoutedEventArgs e)
        {
            TipDisplay tDisplay = new TipDisplay(selected_business.business_id, buildConnString());
            tDisplay.Show();
        }

        private void BusPerCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> categories = new List<string>();
            foreach (string item in SelectedCategoryListBox.Items)
            {
                categories.Add(item);
            }
            BusinessPerCategory chartWindow = new BusinessPerCategory(categories, buildConnString());
            chartWindow.Show();
        }
    }
}

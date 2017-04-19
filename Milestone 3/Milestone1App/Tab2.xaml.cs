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
    }
}

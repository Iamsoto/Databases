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
/*
 *Team: Skankhunt42 
 *  Brian Soto & Edoardo Franco V..
 *  Important:
 *          Our database name is milstone1db not milestone1db
 */
    
    public partial class MainWindow : Window
    {
        // Custombusiness class
        public class Business
        {
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }

        // Configuring the main window
        public MainWindow()
        {
            InitializeComponent();
            addStates();
            addColumns();
        }

        /*
         * I - M -  P -  O -  R -  T -  A  - N - T:
         * Our database is named milstone1db NOT milestone1db (Notice the placement of the first e)
         */ 
        private string buildConnString()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone1DB";
        }

        // Add some Cities to the citylist
        public void addCities()
        {
            CityList.Items.Clear();
            if (StateList.SelectedIndex > -1)
            {
                // Establish connection
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

                    // Establish sql command
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT city FROM business WHERE state = '" + StateList.SelectedItem.ToString() + "' ORDER BY city;";

                        // Add items to citylist
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CityList.Items.Add(reader.GetString(0));
                            }
                        }

                    }
                    conn.Close();
                }
            }
        }

        // Add some states to the statelist
        public void addStates()
        {
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
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";

                    // Add to statelist
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StateList.Items.Add(reader.GetString(0));
                        }
                    }

                }
                conn.Close();
            }
        }

        // Add some columns! 
        public void addColumns()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 255;
            DataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("state");
            DataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            DataGrid.Columns.Add(col3);

        }

        // From online video: state list changed event
        private void StateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.Items.Clear();
            if (StateList.SelectedIndex > -1)
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

                    // Execute sql command
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name,state,city FROM business WHERE state = '" + StateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Add to datagrid
                            while (reader.Read())
                            {
                                DataGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2) });
                            }
                        }

                    }
                    conn.Close();
                }
                addCities();
            }
        }

        // Updates the datagrid when city list selection is changed
        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid.Items.Clear();
            if (StateList.SelectedIndex > -1 && CityList.SelectedIndex > -1)
            {
                // Connect to database
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

                    // Make sql query
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name FROM business WHERE city = '" + CityList.SelectedItem.ToString() + 
                            "' AND state = '" + StateList.SelectedItem.ToString() + "'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            // Add items to the datagrid
                            while (reader.Read())
                            {
                                DataGrid.Items.Add(new Business()
                                { name = reader.GetString(0), state = StateList.SelectedItem.ToString(), city = CityList.SelectedItem.ToString() });
                            }
                        }

                    }
                    conn.Close();
                }
            }
        }
    }
}

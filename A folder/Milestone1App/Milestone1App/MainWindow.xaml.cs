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
 * 
 */
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string name { get; set; }
            public string state { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns();
        }

        private string buildConnString()
        {
            return "Host=localhost:5432; Username=postgres; Password=password; Database = milestone1db;";
        }

        public void addState()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER by state;";
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

            DataGrid.Items.Add(new Business() { name = "business1", state = "WA" });
            DataGrid.Items.Add(new Business() { name = "business2", state = "CA" });
        }
    }
}

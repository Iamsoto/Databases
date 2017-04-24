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

public class CreateConnectionString
{
        public static string build_connection_string()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone2";
        }
}

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
            //addStates();
        }

        private string buildConnString()
        {
            return "Host=127.0.0.1; Username=postgres; Password=password; Database = Milestone2";
        }

        private void tabControl_SelectionChanged()
        {

        }

        private void Tab1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

       
    }
}

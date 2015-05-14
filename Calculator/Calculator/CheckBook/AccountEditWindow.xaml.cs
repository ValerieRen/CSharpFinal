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
using MySql.Data.MySqlClient;

namespace Calculator.CheckBook
{
    /// <summary>
    /// Interaction logic for AccountEditWindow.xaml
    /// </summary>
    public partial class AccountEditWindow : Window
    {
        public AccountEditWindow()
        {
            InitializeComponent();
        }

        private void ShowCheckBookWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
            FillCombobox();
        }

        void FillCombobox()
        {
            string constring = "Database=localhost;username=root;password=root";
            string Query = "select * from database.Account;";

            MySqlConnection conDatabase = new MySqlConnection(constring);
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;

            try
            {
                conDatabase.Open();
                myReader = cmdDatabase.ExecuteReader();
                CheckBookWindow ck = new CheckBookWindow();
                while (myReader.Read())
                {
                    string sAccount = myReader.GetString("Institution");
                    ck.combo.Items.Add(sAccount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

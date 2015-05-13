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
        }
    }
}

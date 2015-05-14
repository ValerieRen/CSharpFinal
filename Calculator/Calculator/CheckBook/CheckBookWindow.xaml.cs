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
    /// Interaction logic for CheckBookWindow.xaml
    /// </summary>
    public partial class CheckBookWindow : Window
    {
        public CheckBookWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var VM = new CheckBookVM();
            DataContext = VM;
            VM.Fill();
            System.Windows.Data.CollectionViewSource transactionViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("transactionViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // transactionViewSource.Source = [generic data source]
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReportForm rp = new ReportForm();
            rp.Show();
        }
    }
}

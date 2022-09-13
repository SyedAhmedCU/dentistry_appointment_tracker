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

//Syed Tasnim Ahmed
namespace dentist_appointment_database
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = VM.Instance;
            DataContext = vm;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new(false) { Owner = this };
            ew.ShowDialog();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedCustomer != null)
            {
                EditWindow ew = new(true) { Owner = this };
                ew.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select an appointment to Edit");
            }
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (vm.SelectedCustomer != null)
            {
                vm.Delete();
            }
            else
            {
                MessageBox.Show("Select a customer to Delete");
            }
        }
    }
}

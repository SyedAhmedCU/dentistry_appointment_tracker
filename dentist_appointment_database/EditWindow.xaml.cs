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

namespace dentist_appointment_database
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        readonly VM vm;
        //creates course object every time
        readonly Customer customer = new();
        public EditWindow(bool isEdit)
        {
            InitializeComponent();
            vm = VM.Instance;

            if (isEdit)
            {
                Title = "Edit Customer Appointment";
                customer.Name = vm.SelectedCustomer.Name;
                customer.PostalCode = vm.SelectedCustomer.PostalCode;
                customer.Phone = vm.SelectedCustomer.Phone;
                customer.Duration = vm.SelectedCustomer.Duration;
                customer.ServiceType = vm.SelectedCustomer.ServiceType;
                customer.ServiceDate = vm.SelectedCustomer.ServiceDate;
                customer.CustomerID = vm.SelectedCustomer.CustomerID;
                customer.AppointmentID = vm.SelectedCustomer.AppointmentID;

                customer.IsNew = false;
            }
            else
                Title = "Add Customer Appointment";
            DataContext = customer;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            vm.Save(customer);
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

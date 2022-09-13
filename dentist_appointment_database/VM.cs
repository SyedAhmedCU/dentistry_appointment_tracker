using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace dentist_appointment_database
{
    internal class VM : INotifyPropertyChanged
    {
        readonly DB db = DB.Instance;
        List<Customer> customers;
        #region singleton
        private static VM vm;
        public static VM Instance { get { vm ??= new VM(); return vm; } }
        #endregion
        #region properties
        public BindingList<Customer> Customers { get; set; } = new();
        private Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { selectedCustomer = value; NotifyPropertyChanged(); }
        }
        private int selectedSortType;
        public int SelectedSortType
        {
            get { return selectedSortType; }
            set { selectedSortType = value; NotifyPropertyChanged(); updateTable(); }
        }
        #endregion
        #region Methods
        private VM()
        {
            bool serviceTypeNotExist = false;
            serviceTypeNotExist = db.AddServiceTypes();
            //if (serviceTypeNotExist)
            //{
            //    MessageBox.Show("serviceType table is populated");
            //}
            //else
            //{
            //    MessageBox.Show("There are data in serviceType table");
            //}
            updateTable();
        }

        public void Save(Customer customer)
        {
            bool success = false;
            if (customer.IsNew)
            {
                success = db.Insert(customer);
                if (!success)
                {
                    MessageBox.Show("Appointment Already Exists");
                }
            }
            else
            {
                success = db.Update(customer);
                if (success)
                {
                    customers.Remove(SelectedCustomer);
                    SelectedCustomer = null;
                }
            }
            if (success)
            {
                customers.Add(customer);
                updateTable();
            }
        }

        public void Delete()
        {
            if (db.Delete(SelectedCustomer))
            {
                Customers.Remove(SelectedCustomer);
                customers.Remove(SelectedCustomer);
                SelectedCustomer = null;
                updateTable();
            }
        }

        public void updateTable()
        {
            //cities = cities.OrderBy(c => c.Name).ToList();
            customers = db.Sort(selectedSortType);
            Customers.Clear();
            foreach (Customer c in customers)
                Customers.Add(c);
        }
        #endregion 
        #region Property Changed
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

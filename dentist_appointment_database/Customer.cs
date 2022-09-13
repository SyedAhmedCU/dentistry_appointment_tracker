using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dentist_appointment_database
{
    internal class Customer
    {
        public bool IsNew { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal Duration { get; set; }
        public int ServiceType { get; set; }
        public int CustomerID { get; set; }
        public int AppointmentID { get; set; }
        public string ServiceDate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
    }
}

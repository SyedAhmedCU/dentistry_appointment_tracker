using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace dentist_appointment_database
{
    internal class DB
    {
        const string CONNECTION_STRING = @"Server=.\SQLEXPRESS;Database=JoesDentistry;Trusted_Connection=True;";
        const string SELECT_COMMAND = "SELECT customers.id as customerID, name, ap.id AS appointmentID, postalCode, phone, ap.duration, ap.serviceDate, ap.serviceTypeId, " +
            "s.description as serviceDescription, (s.ratePerMinute* ap.duration) as totalCost FROM customers " +
            "INNER JOIN appointments ap on customers.id = ap.customerId " +
            "INNER JOIN serviceTypes s on  ap.serviceTypeId = s.id";
        const string CHECK_ADD_SERVICETYPES = "IF NOT EXISTS (SELECT * FROM serviceTypes) " +
            "BEGIN SET IDENTITY_INSERT serviceTypes ON INSERT INTO serviceTypes(id, description, ratePerMinute) " +
            "VALUES (0,'teeth cleaning',0.99), (1, 'filling', 1.99),(2, 'surgery', 7.99), (3, 'All', 10.97); " +
            "SET IDENTITY_INSERT serviceTypes OFF END";

        const string INSERT_COMMAND = "IF NOT EXISTS (SELECT 1 FROM customers WHERE name = @NAME AND id = @CUSTOMERID) " +
            "BEGIN SET IDENTITY_INSERT customers ON " +
            "INSERT INTO customers (id, name, postalCode,phone) " +
            "VALUES (@CUSTOMERID, @NAME, @POSTALCODE, @PHONE) " +
            "SET IDENTITY_INSERT customers OFF END;" +
            "IF NOT EXISTS (SELECT 1 FROM appointments WHERE id = @APPOINTMENTID) " +
            "BEGIN SET IDENTITY_INSERT appointments ON " +
            "INSERT INTO appointments(id, customerId,serviceTypeId, serviceDate, duration) " +
            "VALUES (@APPOINTMENTID, @CUSTOMERID, @SERVICETYPEID, @SERVICEDATE, @DURATION) " +
            "SET IDENTITY_INSERT appointments OFF END";

        const string UPDATE_COMMAND = "UPDATE customers SET name = @NAME, phone = @PHONE, postalCode = @POSTALCODE WHERE id =  @CUSTOMERID " +
            "UPDATE appointments SET customerId = @CUSTOMERID ,serviceTypeId = @SERVICETYPEID, serviceDate = @SERVICEDATE, duration = @DURATION WHERE id  = @APPOINTMENTID";
        const string DELETE_COMMAND = "DELETE FROM appointments WHERE id = @APPOINTMENTID ";


        private readonly SqlConnection conn;
        private static DB db;
        public static DB Instance { get { db ??= new DB(); return db; } }
        private DB()
        {
            conn = new SqlConnection(CONNECTION_STRING);
            conn.Open();
        }
        public bool AddServiceTypes()
        {
            using SqlCommand cmd = new(CHECK_ADD_SERVICETYPES, conn);
            // number of rows affected
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Insert(Customer customer)
        {
            // boolean to see if there was any insert operation
            using SqlCommand cmd = new(INSERT_COMMAND, conn);
            cmd.Parameters.AddWithValue("@NAME", customer.Name);
            cmd.Parameters.AddWithValue("@POSTALCODE", customer.PostalCode);
            cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
            cmd.Parameters.AddWithValue("@CUSTOMERID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@APPOINTMENTID", customer.AppointmentID);
            cmd.Parameters.AddWithValue("@SERVICETYPEID", customer.ServiceType);
            cmd.Parameters.AddWithValue("@SERVICEDATE", customer.ServiceDate);
            cmd.Parameters.AddWithValue("@DURATION", customer.Duration);
            // number of rows affected
            return cmd.ExecuteNonQuery() > 0;
        }
        public bool Update(Customer customer)
        {
            using SqlCommand cmd = new(UPDATE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@NAME", customer.Name);
            cmd.Parameters.AddWithValue("@POSTALCODE", customer.PostalCode);
            cmd.Parameters.AddWithValue("@PHONE", customer.Phone);
            cmd.Parameters.AddWithValue("@CUSTOMERID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@APPOINTMENTID", customer.AppointmentID);
            cmd.Parameters.AddWithValue("@SERVICETYPEID", customer.ServiceType);
            cmd.Parameters.AddWithValue("@SERVICEDATE", customer.ServiceDate);
            cmd.Parameters.AddWithValue("@DURATION", customer.Duration);
            return cmd.ExecuteNonQuery() > 0;
        }
        public bool Delete(Customer customer)
        {
            using SqlCommand cmd = new(DELETE_COMMAND, conn);
            cmd.Parameters.AddWithValue("@APPOINTMENTID", customer.AppointmentID);
            return cmd.ExecuteNonQuery() > 0;
        }

        public List<Customer> Sort(int sortType)
        {
            string SORT_COMMAND = SELECT_COMMAND;
            switch (sortType)
            {
                case 0:
                    SORT_COMMAND += " ORDER BY name ASC";
                    break;
                case 1:
                    SORT_COMMAND += " ORDER BY ap.id ASC";
                    break;
                case 2:
                    SORT_COMMAND += " ORDER BY ap.serviceDate ASC";
                    break;
                case 3:
                    SORT_COMMAND += " ORDER BY totalCost ASC";
                    break;
                default: 
                    break;
            }
            List<Customer> customers = new();

            using SqlCommand cmd = new(SORT_COMMAND, conn);

            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                customers.Add(new Customer
                {
                    CustomerID = dr.GetInt32(dr.GetOrdinal("customerID")),
                    AppointmentID = dr.GetInt32(dr.GetOrdinal("appointmentID")),
                    Name = dr.GetString(dr.GetOrdinal("name")),
                    Phone = dr.GetString(dr.GetOrdinal("phone")),
                    PostalCode = dr.GetString(dr.GetOrdinal("postalCode")),
                    Duration = dr.GetDecimal(dr.GetOrdinal("duration")),
                    ServiceType = dr.GetInt32(dr.GetOrdinal("serviceTypeId")),
                    ServiceDate = dr.GetDateTime(dr.GetOrdinal("serviceDate")).ToString(),
                    Description = dr.GetString(dr.GetOrdinal("serviceDescription")),
                    TotalCost = dr.GetDecimal(dr.GetOrdinal("totalCost")),
                    IsNew = false
                });
            dr.Close();
            return customers;
        }
    }
}

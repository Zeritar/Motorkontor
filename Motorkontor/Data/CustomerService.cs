using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class CustomerService : DbService
    {
        private AddressService addressService;

        public CustomerService(AddressService _addressService)
        {
            addressService = _addressService;
        }

        public Customer[] GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readCustomers", null);

                while (reader.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(reader["customerId"]))
                    {
                        firstName = reader["FirstName"].ToString(),
                        lastName = reader["LastName"].ToString(),
                        createDate = (DateTime)reader["CreateDate"],
                        address = addressService.GetAddresseById(Convert.ToInt32(reader["FK_AddressId"].ToString()))
                    });
                }
            }
            return customers.ToArray();
        }

        public bool PostCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@firstName", customer.firstName),
                    new SqlParameter("@lastName", customer.lastName),
                    new SqlParameter("@addressId", customer.address.addressId)
                };
                return PostProcedure(connection, "usp_postCustomer", parameters);
            }
        }
    }
}

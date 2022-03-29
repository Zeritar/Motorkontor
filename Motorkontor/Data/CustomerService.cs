using System;
using System.Data;
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
                    customers.Add(new Customer(Convert.ToInt32(reader["CustomerId"]))
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

        public Customer GetCustomerById(int id)
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@customerId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readCustomerById", parameters);

                while (reader.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(reader["CustomerId"]))
                    {
                        firstName = reader["FirstName"].ToString(),
                        lastName = reader["LastName"].ToString(),
                        createDate = (DateTime)reader["CreateDate"],
                        address = addressService.GetAddresseById(Convert.ToInt32(reader["FK_AddressId"].ToString()))
                    });
                }
            }
            if (customers.Count > 0)
                return customers[0];

            return null;
        }

        public int PostCustomer(Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@firstName", customer.firstName),
                    new SqlParameter("@lastName", customer.lastName),
                    new SqlParameter("@addressId", customer.address.addressId),
                    new SqlParameter("@id", SqlDbType.Int)
                };
                parameters[parameters.Count - 1].Direction = ParameterDirection.Output;
                PostProcedure(connection, "usp_postCustomer", parameters);
                return (int)parameters[parameters.Count - 1].Value;
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (customer.customerID < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@customerId", customer.customerID),
                    new SqlParameter("@firstName", customer.firstName),
                    new SqlParameter("@lastName", customer.lastName),
                    new SqlParameter("@addressId", customer.address.addressId)
                };
                return PostProcedure(connection, "usp_updateCustomer", parameters);
            }
        }

        public bool DropCustomer(Customer customer)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (customer.customerID < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@customerId", customer.customerID)
                };
                return PostProcedure(connection, "usp_dropCustomer", parameters);
            }
        }
    }
}

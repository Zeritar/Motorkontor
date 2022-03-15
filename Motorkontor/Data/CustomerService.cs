using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Motorkontor.Data
{
    public class CustomerService : DbService
    {
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
                        createDate = (DateTime)reader["CreateDate"]
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
                    new SqlParameter("@lastName", customer.lastName)
                };
                return PostProcedure(connection, "usp_postCustomer", parameters);
            }
        }

        public SqlDataReader GetProcedure(SqlConnection connection, string procedure, List<SqlParameter>? parameters)
        {
            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Count > 0)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            connection.Open();
            return cmd.ExecuteReader();
        }

        public bool PostProcedure(SqlConnection connection, string procedure, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null && parameters.Count > 0)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            else
            {
                return false; // Can't insert with no parameters
            }
            connection.Open();
            return (cmd.ExecuteNonQuery() > 0) ? true : false;
        }
    }
}

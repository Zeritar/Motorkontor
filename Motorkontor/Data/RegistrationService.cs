using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class RegistrationService : DbService
    {
        private CustomerService customerService;
        private VehicleService vehicleService;

        public RegistrationService(CustomerService _customerService, VehicleService _vehicleService)
        {
            customerService = _customerService;
            vehicleService = _vehicleService;
        }

        public Registration[] GetRegistrations()
        {
            List<Registration> registrations = new List<Registration>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readRegistrations", null);

                while (reader.Read())
                {
                    registrations.Add(new Registration(Convert.ToInt32(reader["RegistrationId"]))
                    {
                        registrationDate = (DateTime)reader["RegistrationDate"],
                        customer = customerService.GetCustomerById(Convert.ToInt32(reader["FK_CustomerId"].ToString())),
                        vehicle = vehicleService.GetVehicleById(Convert.ToInt32(reader["FK_VehicleId"].ToString()))

                    });
                }
            }
            return registrations.ToArray();
        }

        public Registration GetRegistrationById(int id)
        {
            List<Registration> registrations = new List<Registration>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@registrationId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readRegistrationById", parameters);

                while (reader.Read())
                {
                    registrations.Add(new Registration(Convert.ToInt32(reader["RegistrationId"]))
                    {
                        registrationDate = (DateTime)reader["RegistrationDate"],
                        customer = customerService.GetCustomerById(Convert.ToInt32(reader["FK_CustomeryId"].ToString())),
                        vehicle = vehicleService.GetVehicleById(Convert.ToInt32(reader["FK_VehicleId"].ToString()))

                    });
                }
            }
            if (registrations.Count > 0)
                return registrations[0];

            return null;
        }

        public int PostRegistration(Registration registration)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@registrationDate", registration.registrationDate),
                    new SqlParameter("@customerId", registration.customer.customerID),
                    new SqlParameter("@vehicleId", registration.vehicle.vehicleId),
                    new SqlParameter("@id", SqlDbType.Int)
                };
                parameters[parameters.Count - 1].Direction = ParameterDirection.Output;
                PostProcedure(connection, "usp_postRegistration", parameters);
                return (int)parameters[parameters.Count - 1].Value;
            }
        }

        public bool UpdateRegistration(Registration registration)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@registrationId", registration.registrationId),
                    new SqlParameter("@registrationDate", registration.registrationDate),
                    new SqlParameter("@customerId", registration.customer.customerID),
                    new SqlParameter("@vehicleId", registration.vehicle.vehicleId)
                };
                return PostProcedure(connection, "usp_updateRegistration", parameters);
            }
        }

        public bool DropRegistration(Registration registration)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@registrationId", registration.registrationId)
                };
                return PostProcedure(connection, "usp_dropRegistration", parameters);
            }
        }
    }
}

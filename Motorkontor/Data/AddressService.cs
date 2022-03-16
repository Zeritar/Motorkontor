using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class AddressService : DbService
    {
        private ZipCodeService zipCodeService;

        public AddressService(ZipCodeService _zipCodeService)
        {
            zipCodeService = _zipCodeService;
        }

        public Address[] GetAddresses()
        {
            List<Address> addresses = new List<Address>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readAddresses", null);

                while (reader.Read())
                {
                    addresses.Add(new Address(Convert.ToInt32(reader["AddressId"]))
                    {
                        streetAndNo = reader["StreetAndNo"].ToString(),
                        zipCode = zipCodeService.GetZipCodeById(Convert.ToInt32(reader["FK_ZipcodeId"].ToString())),
                        createDate = (DateTime)reader["CreateDate"]
                    });
                }
            }
            return addresses.ToArray();
        }

        public Address GetAddresseById(int id)
        {
            List<Address> addresses = new List<Address>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@addressId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readAddressById", parameters);

                while (reader.Read())
                {
                    addresses.Add(new Address(Convert.ToInt32(reader["AddressId"]))
                    {
                        streetAndNo = reader["StreetAndNo"].ToString(),
                        zipCode = zipCodeService.GetZipCodeById(Convert.ToInt32(reader["FK_ZipcodeId"].ToString())),
                        createDate = (DateTime)reader["CreateDate"]
                    });
                }
            }
            if (addresses.Count > 0)
                return addresses[0];

            return null;
        }

        public bool PostAddress(Address address)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@streetAndNo", address.streetAndNo),
                    new SqlParameter("@zipcodeId", address.zipCode.zipCodeId)
                };
                return PostProcedure(connection, "usp_postAddress", parameters);
            }
        }
    }
}

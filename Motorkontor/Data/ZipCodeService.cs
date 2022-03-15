using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class ZipCodeService : DbService
    {
        public ZipCode[] GetZipCodes()
        {
            List<ZipCode> zipCodes = new List<ZipCode>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readZipcodes", null);

                while (reader.Read())
                {
                    zipCodes.Add(new ZipCode(Convert.ToInt32(reader["ZipcodeId"]))
                    {
                        zipCodeName = reader["ZipcodeName"].ToString(),
                        cityName = reader["CityName"].ToString()
                    });
                }
            }
            return zipCodes.ToArray();
        }

        public ZipCode GetZipCodeById(int id)
        {
            List<ZipCode> zipCodes = new List<ZipCode>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@zipcodeId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readZipcodeById", parameters);

                while (reader.Read())
                {
                    zipCodes.Add(new ZipCode(Convert.ToInt32(reader["ZipcodeId"]))
                    {
                        zipCodeName = reader["ZipcodeName"].ToString(),
                        cityName = reader["CityName"].ToString()
                    });
                }
            }
            if (zipCodes.Count > 0)
                return zipCodes[0];

            return null;
        }

        public bool PostZipCode(ZipCode zipCode)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@zipcodeName", zipCode.zipCodeName),
                    new SqlParameter("@cityName", zipCode.cityName)
                };
                return PostProcedure(connection, "usp_postZipcode", parameters);
            }
        }
    }
}

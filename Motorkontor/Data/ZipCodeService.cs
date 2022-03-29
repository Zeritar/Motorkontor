using System;
using System.Data;
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

        public int PostZipCode(ZipCode zipCode)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@zipcodeName", zipCode.zipCodeName),
                    new SqlParameter("@cityName", zipCode.cityName),
                    new SqlParameter("@id", SqlDbType.Int)
                };
                parameters[parameters.Count - 1].Direction = ParameterDirection.Output;
                PostProcedure(connection, "usp_postZipcode", parameters);
                return (int)parameters[parameters.Count - 1].Value;
            }
        }

        public bool UpdateZipCode(ZipCode zipCode)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (zipCode.zipCodeId < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@zipcodeId", zipCode.zipCodeId),
                    new SqlParameter("@zipcodeName", zipCode.zipCodeName),
                    new SqlParameter("@cityName", zipCode.cityName)
                };
                return PostProcedure(connection, "usp_updateZipcode", parameters);
            }
        }

        public bool DropZipCode(ZipCode zipCode)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (zipCode.zipCodeId < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@zipcodeId", zipCode.zipCodeId)
                };
                return PostProcedure(connection, "usp_dropZipcode", parameters);
            }
        }
    }
}

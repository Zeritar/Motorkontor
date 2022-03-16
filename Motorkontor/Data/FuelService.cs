using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class FuelService : DbService
    {
        public Fuel[] GetFuels()
        {
            List<Fuel> fuels = new List<Fuel>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlDataReader reader = GetProcedure(connection, "usp_readFuels", null);

                while (reader.Read())
                {
                    fuels.Add(new Fuel(Convert.ToInt32(reader["FuelId"]))
                    {
                        fuelName = reader["FuelName"].ToString()
                    });
                }
            }
            return fuels.ToArray();
        }

        public Fuel GetFuelById(int id)
        {
            List<Fuel> fuels = new List<Fuel>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@fuelId", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readFuelById", parameters);

                while (reader.Read())
                {
                    fuels.Add(new Fuel(Convert.ToInt32(reader["FuelId"]))
                    {
                        fuelName = reader["FuelName"].ToString()
                    });
                }
            }
            if (fuels.Count > 0)
                return fuels[0];

            return null;
        }

        public bool PostFuel(Fuel fuel)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@fuelName", fuel.fuelName),
                };
                return PostProcedure(connection, "usp_postFuel", parameters);
            }
        }
    }
}

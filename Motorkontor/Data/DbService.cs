using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System;

namespace Motorkontor.Data
{
    public class DbService
    {
        public string connStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=Motorkontor;Integrated Security=True;TrustServerCertificate=True";

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
            try
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
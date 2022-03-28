using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class LoginService : DbService
    {
        public Login GetLoginById(int id)
        {
            List<Login> logins = new List<Login>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", id)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readLoginById", parameters);

                while (reader.Read())
                {
                    logins.Add(new Login(Convert.ToInt32(reader["Id"]))
                    {
                        username = reader["UserName"].ToString()
                    });
                }
            }
            if (logins.Count > 0)
                return logins[0];

            return null;
        }

        public Login Login(string username, string password)
        {
            List<Login> logins = new List<Login>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@username", username),
                    new SqlParameter("@passwd", password)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readLoginByUserPass", parameters);

                while (reader.Read())
                {
                    logins.Add(new Login(Convert.ToInt32(reader["Id"]))
                    {
                        username = reader["UserName"].ToString()
                    });
                }
            }
            if (logins.Count > 0)
                return logins[0];

            return null;
        }

        public bool PostLogin(Login login)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@username", login.username),
                    new SqlParameter("@paswd", login.password)
                };
                return PostProcedure(connection, "usp_postLogin", parameters);
            }
        }

        public bool UpdateLogin(Login login)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (login.id < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", login.id),
                    new SqlParameter("@username", login.username),
                    new SqlParameter("@passwd", login.password)
                };
                return PostProcedure(connection, "usp_updateLogin", parameters);
            }
        }

        public bool DropLogin(Login login)
        {
            // No need to bother the database if the object didn't originate there. Newly created objects have an ID of 0
            if (login.id < 1)
                return false;

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@id", login.id)
                };
                return PostProcedure(connection, "usp_dropLogin", parameters);
            }
        }
    }
}

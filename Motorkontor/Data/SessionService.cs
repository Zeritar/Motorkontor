using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Motorkontor.Data
{
    public class SessionService : DbService
    {

        private LoginService loginService;

        public SessionService(LoginService _loginService)
        {
            loginService = _loginService;
        }

        public Session GetSessionByGuid(string guid)
        {
            List<Session> sessions = new List<Session>();
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sessionGuid", guid)
                };

                SqlDataReader reader = GetProcedure(connection, "usp_readSessionByGuid", parameters);

                while (reader.Read())
                {
                    sessions.Add(new Session(reader["SessionGuid"].ToString())
                    {
                        login = loginService.GetLoginById(Convert.ToInt32(reader["FK_LoginId"].ToString())),
                        startTime = (DateTime)reader["StartTime"]
                    });
                }
            }
            if (sessions.Count > 0)
                return sessions[0];

            return null;
        }

        public bool PostSession(Session session)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sessionGuid", session.gUID),
                    new SqlParameter("@userId", session.login.id)
                };
                return PostProcedure(connection, "usp_postSession", parameters);
            }
        }

        public bool UpdateSession(Session session)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sessionGuid", session.gUID)
                };
                return PostProcedure(connection, "usp_refreshSession", parameters);
            }
        }

        public bool DropSession(Session session)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@sessionGuid", session.gUID)
                };
                return PostProcedure(connection, "usp_dropSession", parameters);
            }
        }
    }
}

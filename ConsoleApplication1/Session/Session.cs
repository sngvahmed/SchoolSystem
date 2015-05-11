using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// developed by nasser

namespace ERP.Session
{
    interface SessionInterface {
        bool saveSession(SessionDomain sessionDomain);
        bool updateSession(SessionDomain sessionDomain);
        SessionDomain getSession(string title);
    }

    class SessionDomain {
        public string title;
        public string description;
        public int classID;
        public int dateID;
        public int schedualeID;
        public DateTime date;

        public SessionDomain(string title , string description , int classID , int schedualeID , int dateID , DateTime date) {
            this.title = title;
            this.description = description;
            this.classID = classID;
            this.dateID = dateID;
            this.schedualeID = schedualeID;
            this.date = date;
        }

        public SessionDomain(string title, string description, int classID, int schedualeID, int dateID) {
            this.title = title;
            this.description = description;
            this.classID = classID;
            this.dateID = dateID;
            this.schedualeID = schedualeID;
            this.date = DateTime.UtcNow; 
        }

        public void printDate(){
            Console.WriteLine("title is :: " + title);
            Console.WriteLine("description is :: " + description);
            Console.WriteLine("classId is :: " + classID);
            Console.WriteLine("dateID is :: " + dateID);
            Console.WriteLine("schedialeID is :: " + schedualeID);
            Console.WriteLine("date :: " + date);
        }
    }

    class Session : SessionInterface {
        private SessionQuery sessionQuery;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public Session() {
            sqlConnection = new SqlConnection("Server=.\\SQLEXPRESS;Database=ERP;Integrated Security=true");
            sessionQuery = new SessionQuery();

        }

        public bool saveSession(SessionDomain sessionDomain) {
            bool check = false;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(sessionQuery.saveSessionQuery(sessionDomain), sqlConnection);
            try {
                sqlCommand.ExecuteNonQuery();
                check = true;
            } catch (InvalidCastException invalid) {
                Console.WriteLine(invalid.Message);
            } catch (SqlException sql) {
                Console.WriteLine(sql.Message);
            } catch (InvalidOperationException invalid) {
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return check;
        }

        public bool updateSession(SessionDomain sessionDomain) {
            bool ret = false;
            sqlConnection.Open();
            string query = sessionQuery.updateSessionQuery(sessionDomain) ;
            Console.WriteLine(query);
            sqlCommand = new SqlCommand(query , sqlConnection);
            try {
                sqlCommand.ExecuteNonQuery();
                ret = true;
            } catch (InvalidCastException invalid) {
                Console.WriteLine(invalid.Message);
            } catch (SqlException sql) {
                Console.WriteLine(sql.Message);
            } catch (InvalidOperationException invalid) {
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return ret;
        }

        public SessionDomain getSession(string title) {
            SessionDomain sessionDomain = null; 
            sqlConnection.Open();
            string query = sessionQuery.getSessionQuery(title);
            sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Console.WriteLine("accept");
                    sessionDomain = new SessionDomain(title, sqlDataReader.GetString(0),
                                                sqlDataReader.GetInt32(1), sqlDataReader.GetInt32(2),
                                                sqlDataReader.GetInt32(3), sqlDataReader.GetDateTime(4));
                }
                sqlDataReader.Close();
            }
            catch (InvalidCastException invalid)
            {
                Console.WriteLine(invalid.Message);
            }
            catch (SqlException sql)
            {
                Console.WriteLine("exception sql is :: " + sql.Message);
            }
            catch (InvalidOperationException invalid)
            {
                Console.WriteLine(invalid.Message);
            }
            catch (Exception exception) {
                Console.WriteLine(exception.Message);
            }

            sqlConnection.Close();
            return sessionDomain;
        }

    }

    class SessionQuery {
        public string saveSessionQuery(SessionDomain session) { 
            return "INSERT INTO [dbo].[Session] ([title] ,[description] ,[Date] ,[classID] ,[schedualID] ,[dateID]) " +
                   "VALUES ('" + session.title +  "','" + session.description + "','" + session.date +  "','" + session.classID + 
                   "','" + session.schedualeID +  "','" + session.dateID +"')";
        }

        public string updateSessionQuery(SessionDomain session) {
            return "UPDATE [dbo].[Session] SET [description] = '" + session.description + "' ,[Date] = '" + session.date +
                   "',[classID] = '" + session.classID + "',[schedualID] = '" + session.schedualeID + "',[dateID] = '" + session.dateID +
                   "' WHERE title = '" + session.title + "'";
        }

        public string getSessionQuery(string title) {
            return "SELECT [description] ,[classID] ,[schedualID] ,[dateID] ,[Date] FROM [dbo].[Session] WHERE title = '" + title + "';";
        }

    }
}

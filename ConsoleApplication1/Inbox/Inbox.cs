using ERP.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* developed by ahmed nasser */

namespace ERP.Inbox{
    interface InboxInterface{
        bool insert(InboxDomain user);
        bool delete(string to , string from);
        InboxDomain getInbox(string to , string from);
    }

    class InboxDomain{
        public string to;
        public string from;
        public string subject;
        public string detail;
        public DateTime date;

        public InboxDomain(string to , string from , string subject , string detail) {
            this.to = to;
            this.from = from;
            this.subject = subject;
            this.detail = detail;
            date = DateTime.UtcNow;
        }
        
        public InboxDomain(string to, string from, string subject, string detail , DateTime dateTime){
            this.to = to;
            this.from = from;
            this.subject = subject;
            this.detail = detail;
            this.date = dateTime;
        }
    }

    class Inbox : InboxInterface{
        private InboxQuery inboxQuery;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public Inbox() {
            sqlConnection = new SqlConnection("Server=.\\SQLEXPRESS;Database=ERP;Integrated Security=true");
            inboxQuery = new InboxQuery();
        }

        public bool insert(InboxDomain inbox) {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(inboxQuery.insertInboxQuery(inbox), sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
            }
            catch (InvalidCastException invalid)
            {
                Console.WriteLine(invalid.Message);
            }
            catch (SqlException sql)
            {
                Console.WriteLine(sql.Message);
            }
            catch (InvalidOperationException invalid)
            {
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return true;
        }

       public bool delete(string to, string from) {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(inboxQuery.deleteInboxQuery(to , from), sqlConnection);
            try{
                sqlCommand.ExecuteNonQuery();
            }catch (InvalidCastException invalid){
                Console.WriteLine(invalid.Message);
            }catch (SqlException sql){
                Console.WriteLine(sql.Message);
            }catch (InvalidOperationException invalid){
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return true;
        }

        public InboxDomain getInbox(string to, string from) {
            InboxDomain inbox = null;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(inboxQuery.getInboxQueryOneUser(from , to), sqlConnection);
            try{
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    DateTime date = sqlDataReader.GetDateTime(0);
                    inbox = new InboxDomain(to, from, sqlDataReader.GetString(1), sqlDataReader.GetString(2), date);
                }
                sqlDataReader.Close();
            
            }catch (InvalidCastException invalid){
                Console.WriteLine(invalid.Message);
            }catch (SqlException sql){
                Console.WriteLine(sql.Message);
            }catch (InvalidOperationException invalid){
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return inbox; 
        }
    }

    class InboxQuery {
        public string insertInboxQuery(InboxDomain inbox) {
            return "INSERT INTO [dbo].[Inbox] ([toInbox] ,[fromInbox] ,[Date],[subject],[detail])" +
                   "VALUES ('"+inbox.to+"','"+inbox.from+"','"+inbox.date+"','"+inbox.subject+"','"+inbox.detail+"')";
        }

        public string deleteInboxQuery(string to , string from) {
            return "DELETE FROM [dbo].[Inbox] WHERE toInbox='" + to + "' AND fromInbox='" + from + "';";
        }

        public string getInboxQueryOneUser(string from , string to) {
            return "SELECT [Date] ,[subject] ,[detail] FROM [dbo].[Inbox] WHERE toInbox='" + to + "' AND fromInbox='" + from + "'";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/* developed by ahmed nasser */
namespace ERP.User {
    
    interface UserInterface {
        bool update(UserDomain user);
        bool insertUser(UserDomain user);
        bool delete(string username);
        UserDomain getUser(string username);
        bool exist_or(string email, string password);
    }

    class UserDomain { 
        
        public string permission;
        public string gender;
        public string firstName;
        public string secondName;
        public string password;
        public string address;
        public string email;
        public int age;

        public UserDomain(string pass, string email, string per, string fname, string sname, string add, int age , string gen) {
            permission = per;
            gender = gen;
            firstName = fname;
            secondName = sname;
            password = pass;
            address = add;
            this.age = age;
            this.email = email;
        }

        public void printInYou() {
            string ageS = Convert.ToString(age) ;
            Console.WriteLine(" password is :: " + password + " and age is :: " + ageS);
            Console.WriteLine("first name is :: " + firstName + " and second name is :: " + secondName);
            Console.WriteLine("email is :: " + email + " and permission is :: " + permission);
            Console.WriteLine("gender is :: " + gender + " and address is :: " + address );
        }
    }

    class User : UserInterface {
        private UserQuery userQuery;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public User() { 
            sqlConnection = new SqlConnection("Server=.\\SQLEXPRESS;Database=ERP;Integrated Security=true"); 
            userQuery = new UserQuery();
        }

        public UserDomain getUser(string username){
            UserDomain userDomain = null;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(userQuery.getUserQuery(username), sqlConnection);
            try {
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read()){
                    userDomain = new UserDomain(sqlDataReader.GetString(0), sqlDataReader.GetString(1),
                                                sqlDataReader.GetString(2), sqlDataReader.GetString(3),
                                                sqlDataReader.GetString(4), sqlDataReader.GetString(5),
                                                sqlDataReader.GetInt32(6), sqlDataReader.GetString(7));
                }
                sqlDataReader.Close();
            }catch(InvalidCastException invalid){
                Console.WriteLine(invalid.Message);
            }catch(SqlException sql){
                Console.WriteLine(sql.Message);
            }catch (InvalidOperationException invalid){
                Console.WriteLine(invalid.Message);
            }

            sqlConnection.Close();
            return userDomain;
        }


        public bool delete(string username) {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(userQuery.deleteQuery(username), sqlConnection);
            try{
                sqlCommand.ExecuteNonQuery();
            }catch(InvalidCastException invalid){
                Console.WriteLine(invalid.Message);
            }catch(SqlException sql){
                Console.WriteLine(sql.Message);
            }catch (InvalidOperationException invalid){
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return true;
        }

        public bool update(UserDomain user) {
            bool check = false;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(userQuery.updateQuery(user), sqlConnection);
            try { 
                sqlCommand.ExecuteNonQuery();
                check = true;
            }catch(InvalidCastException invalid){
                Console.WriteLine(invalid.Message);
            }catch(SqlException sql){
                Console.WriteLine(sql.Message);
            }catch (InvalidOperationException invalid){
                Console.WriteLine(invalid.Message);
            }
            sqlConnection.Close();
            return check;
        }

        public bool insertUser(UserDomain user) {
            sqlConnection.Open();
            sqlCommand = new SqlCommand(userQuery.insertQuery(user), sqlConnection);
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

        public bool exist_or(string email, string password) {
            bool ret = false;

            sqlConnection.Open();
            sqlCommand = new SqlCommand(userQuery.existQuery(email , password), sqlConnection);
            
            try{
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read()) { ret = true; }
                sqlDataReader.Close();
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

    }

     class UserQuery {
        public string insertQuery(UserDomain userDomain) {
            return "INSERT INTO [dbo].[User] ([password] ,[email]" +
                   ",[permission] ,[firstName] ,[secondName] ,[address] ,[age] ,[gender]) VALUES ('" +
                   userDomain.password + "','" + userDomain.email + "','" + userDomain.permission 
                   + "','" + userDomain.firstName + "','" + userDomain.secondName + "','" +
                   userDomain.address + "','" + Convert.ToString(userDomain.age) + "','" + userDomain.gender + "')";
        }

        public string deleteQuery(string email){
            return " DELETE FROM [dbo].[User] WHERE email = '" + email + "'";
        }

        public string getUserQuery(string email){
            return "SELECT [password] ,[email] ,[permission] " +
                   ",[firstName] ,[secondName] ,[address] ,[age] ,[gender] " +
                   "FROM [dbo].[User] WHERE email = '" + email + "'";
        }

        public string updateQuery(UserDomain userDomain){
            return "UPDATE [dbo].[User] SET " +
                   " [password] = '" + userDomain.password + "' " +
                   " , [permission] = '" + userDomain.permission + "' " +
                   " , [firstName] = '" + userDomain.firstName + "' " +
                   " , [secondName] = '" + userDomain.secondName + "' " +
                   " , [address] = '" + userDomain.address + "' " +
                   " , [age] = '" + Convert.ToString(userDomain.age) + "' " +
                   " , [gender] = '" + userDomain.gender + "' " +
                   " WHERE email = '" + userDomain.email + "'";
        }

        public string existQuery(string email , string password) { 
            return "SELECT [email] FROM [dbo].[User] WHERE email = '"+email+"' AND password = '"+password+"'";
        }
    }
}

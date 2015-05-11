using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// developed by nasser

namespace ERP.Course
{
    interface CourseInterface
    {
        bool insert(string coursename);
        bool insert(CourseDomain courseDomain);
        bool update(string coursenewName , int id);
        bool update(CourseDomain courseDomain);
        CourseDomain get(string course);
        CourseDomain get(int id);
    }

    class CourseDomain
    {
        public int id;
        public string coursename;
        public CourseDomain(string coursename){
            this.coursename = coursename;
        }
        public CourseDomain(string coursename , int id)
        {
            this.coursename = coursename;
            this.id = id;
        }

    }

    class Course : CourseInterface
    {
        private CourseQuery courseQuery;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDataReader sqlDataReader;

        public Course()
        {
            sqlConnection = new SqlConnection("Server=.\\SQLEXPRESS;Database=ERP;Integrated Security=true");
            courseQuery = new CourseQuery();
        }

        public bool insert(CourseDomain courseDomain) {
            bool check = false;
            sqlConnection.Open();
            string query = courseQuery.insertQuery(courseDomain.coursename);
            sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                check = true;
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
            return check;
        }

        public bool insert(string coursename) {
            bool check = false;
            sqlConnection.Open();
            string query = courseQuery.insertQuery(coursename);
            sqlCommand = new SqlCommand(query , sqlConnection);
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

        public bool update(CourseDomain courseDomain) {
            bool ret = false;
            sqlConnection.Open();
            string query = courseQuery.updateQuery(courseDomain.coursename, courseDomain.id);
            sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                ret = true;
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
            return ret;
        }
        public bool update(string coursenewName , int id) {
            bool ret = false;
            sqlConnection.Open();
            string query = courseQuery.updateQuery(coursenewName , id);
            sqlCommand = new SqlCommand(query, sqlConnection);
            try
            {
                sqlCommand.ExecuteNonQuery();
                ret = true;
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
            return ret;
        }

        public CourseDomain get(string course)
        {
            CourseDomain courseDomain = null;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(courseQuery.getCourseByname(course), sqlConnection);
            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    int id = sqlDataReader.GetInt32(0);
                    string name = sqlDataReader.GetString(1);
                    courseDomain = new CourseDomain(name , id);
                }
                sqlDataReader.Close();

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
            return courseDomain; 
        }

        public CourseDomain get(int id)
        {
            CourseDomain courseDomain = null;
            sqlConnection.Open();
            sqlCommand = new SqlCommand(courseQuery.getCourseByid(id), sqlConnection);
            try
            {
                sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    id = sqlDataReader.GetInt32(0);
                    string name = sqlDataReader.GetString(1);
                    courseDomain = new CourseDomain(name, id);
                }
                sqlDataReader.Close();

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
            return courseDomain; 
        }
    }

    class CourseQuery{
        public string insertQuery(string coursename)
        {
            return "INSERT INTO [dbo].[Course] ([Name]) VALUES ('"+coursename+"')";
        }

        public string updateQuery(string coursenewName , int id) {
            return "UPDATE [dbo].[Course] SET [Name] = '" + coursenewName + "' WHERE ID = '" + id + "'";
        }

        public string getCourseByname(string name) { 
            return "SELECT [ID] ,[Name] FROM [dbo].[Course] WHERE Name = '" + name + "'";
        }
        
        public string getCourseByid(int name)
        {
            return "SELECT [ID] ,[Name] FROM [dbo].[Course] WHERE id = '" + name + "'";
        }
    }
}

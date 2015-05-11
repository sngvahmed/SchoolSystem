﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// developed by nasser

namespace ERP.Cousre
{
    interface CourseInterface
    {
        bool insert(string coursename);
        bool update(string coursenewName , int id);
    }

    class CourseDomain
    {
        public int id;
        public string coursename;
        public CourseDomain(string coursename){
            this.coursename = coursename;
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

        public bool update(string coursenewName , int id) {
            bool ret = false;
            sqlConnection.Open();
            string query = courseQuery.updateQuery(coursenewName , id);
            Console.WriteLine(query);
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
    }
}

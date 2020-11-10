using HTTP5101Assignment3.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HTTP5101Assignment3.Controllers
{
    // HTTP5101 B Assignment 3: A WebAPI Controller which allows you to access 
    // information about teachers.
    public class TeacherDataController : ApiController
    {
        // This object will handle the interactions with the
        // database.
        private SchoolDbContext schoolDb = new SchoolDbContext();

        /// <summary>
        /// This function is called in order to determine the highest teacher ID
        /// in the database for validation in the feature allowing the user to 
        /// find a specific teacher by their ID.
        /// </summary>
        /// <returns>A positive integer that represents the highest teacher ID in
        /// the database.
        /// </returns>
        /// <example>
        /// GET api/TeacherData/getHighestTeacherId
        /// </example>
        [HttpGet]
        public int getHighestTeacherId()
        {
            // Create and open a connection to the database.
            MySqlConnection connection = schoolDb.accessDatabase();
            connection.Open();

            // Create and set a query for the database that will retrieve
            // the teachers' first and last names.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT teacherid FROM teachers ORDER BY teacherid DESC LIMIT 1";

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Declare an int to hold the highest id. Initialize it to
            // 0.
            int maxTeacherId = 0;

            // Read the results. There should be only one row.
            while( results.Read() ) {
                maxTeacherId = results.GetInt32( results.GetOrdinal( "teacherid" ) );
            }

            // Close the connection.
            connection.Close();

            return maxTeacherId;
        }

        /// <summary>
        /// Returns a list of teachers in the School database.
        /// </summary>
        /// <returns>
        /// A list of the teachers' first and last names.
        /// </returns>
        /// <example>
        /// GET api/TeacherData/listTeachers
        /// </example>
        /// 
        [HttpGet]
        public IEnumerable<String> listTeachers() 
        {
            // Create and open a connection to the database.
            MySqlConnection connection = schoolDb.accessDatabase();
            connection.Open();

            // Create and set a query for the database that will retrieve
            // the teachers' first and last names.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT teacherfname, teacherlname FROM teachers";

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Create a list object to hold the teacher names.
            List<String> teachers = new List<string>();

            // Read the results, one row at a time.
            while( results.Read() ) {

                // The results will have the same column names as
                // the database since we did no alias them.
                // Create a string with the first and last names together
                // and add them to the teachers list.
                teachers.Add( results[ "teacherfname" ] + " " + results[ "teacherlname" ] );
            }

            // Close the connection to the database.
            connection.Close();

            // Return the list of teachers.
            return teachers;
        }

        /// <summary>
        /// Returns a Teacher object constructed with data for the teacher
        /// with the given id in the School database.
        /// </summary>
        /// <returns>
        /// A Teacher object with the teacher's information if a teacher 
        /// with the given id exists in the database, otherwise null.
        /// </returns>
        /// <example>
        /// GET api/TeacherData/getTeacher/{id}
        /// </example>
        /// 
        [Route( "api/TeacherData/getTeacher/{id}" )]
        public Teacher getTeacher( int? id )
        {
            // Create and open a connection to the database.
            MySqlConnection connection = schoolDb.accessDatabase();
            connection.Open();

            // Create and set a query for the database that will retrieve
            // the teachers' first and last names.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM teachers WHERE teacherid = " + id;

            // Create an object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Create a Teacher object for the function to return.
            // If no teacher was found, the members of the object
            // will be uninitialized.
            Teacher teacher = new Teacher();

            // Read the results. There should be only one row.
            while( results.Read() ) {
                teacher.employeeNumber = results[ "employeenumber" ].ToString();
                teacher.firstName = results[ "teacherfname" ].ToString();
                teacher.lastName = results[ "teacherlname" ].ToString();
                teacher.teacherId = results.GetInt32( results.GetOrdinal( "teacherid" ) );
                teacher.salary = results.GetDecimal( results.GetOrdinal( "salary" ) );
                teacher.hireDate = results.GetDateTime( results.GetOrdinal( "hiredate" ) );
            }

            // Close the connection.
            connection.Close();

            return teacher;
        }

    }
}

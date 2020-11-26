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
    // This class is heavily influenced by https://github.com/christinebittle/BlogProject_1/blob/master/BlogProject/Controllers/AuthorDataController.cs
    // as cloned on 2020/11/04.

    public class TeacherDataController: SchoolObjectDataController
    {
        /// <summary>
        /// This function is called in order to determine the highest teacher ID
        /// in the database for validation in the feature allowing the user to 
        /// find a specific teacher by their ID.
        /// </summary>
        /// <returns>A positive integer that represents the highest teacher ID in
        /// the database, or a 0 if unable to connect to the database.
        /// </returns>
        /// <example>
        /// GET api/TeacherData/getHighestTeacherId
        /// </example>
        [HttpGet]
        public Teacher.MaxId getHighestTeacherId()
        {
            // Create and open a connection to the database.
            MySqlConnection connection = getConnection();
            if( connection == null ) {

                // Return 0 if the connection was not successful.
                return new Teacher.MaxId(0);
            }

            // Create and set a query for the database that will retrieve
            // the teachers' first and last names.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT teacherid FROM teachers ORDER BY teacherid DESC LIMIT 1";

            // Create a and object to hold the reader of the command
            // and execute.
            MySqlDataReader reader = command.ExecuteReader();

            // Declare an int to hold the highest id. Initialize it to
            // 0.
            int maxTeacherId = 0;

            // Read the reader. There should be only one row.
            while( reader.Read() ) {
                maxTeacherId = reader.GetInt32( reader.GetOrdinal( "teacherid" ) );
            }

            // Close the connection.
            connection.Close();

            // Return the max teacher ID.
            return new Teacher.MaxId( maxTeacherId );
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
        [HttpGet]
        [Route( "api/TeacherData/listTeachers" )]
        public IEnumerable<Teacher> listTeachers()
        {
            return (IEnumerable<Teacher>) list( "teachers" );
        }

        override
        protected IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader )
        {
            // Create a list object to hold the teacher names.
            List<Teacher> teachers = new List<Teacher>();

            // Read the reader, one row at a time.
            while( reader.Read() ) {

                // Create a new Teacher object.
                Teacher teacher = new Teacher();

                // The reader will have the same column names as
                // the database since we did not alias them.
                // Put the data in the corresponding field of the Teacher
                // object.
                teacher.employeeNumber = reader[ "employeenumber" ].ToString();
                teacher.firstName = reader[ "teacherfname" ].ToString();
                teacher.lastName = reader[ "teacherlname" ].ToString();
                teacher.teacherId = reader.GetInt32( reader.GetOrdinal( "teacherid" ) );
                teacher.salary = reader.GetDecimal( reader.GetOrdinal( "salary" ) );
                teacher.hireDate = reader.GetDateTime( reader.GetOrdinal( "hiredate" ) );

                // Add the teacher to the list.
                teachers.Add( teacher );
            }

            return teachers;
        }

        /// <summary>
        /// Returns a Teacher object constructed with data for the teacher
        /// with the given id in the School database. If the id is null or 
        /// the teacher not found, the object will be uninitialized
        /// </summary>
        /// <input>An integer value representing the teacher's ID.</input>
        /// <returns>
        /// A Teacher object with the teacher's information if a teacher 
        /// with the given id exists in the database, otherwise an uninitialized
        /// Teacher object..
        /// </returns>
        /// <example>
        /// GET api/TeacherData/getTeacher/{id}
        /// </example>
        /// <example>
        /// GET api/TeacherData/getTeacher
        /// </example>
        [HttpGet]
        [Route( "api/TeacherData/getTeacher/{id}" )]
        public Teacher getTeacher( int? id )
        {
            return (Teacher) get( "teachers", "teacherid", id );
        }

        private IEnumerable<Class> getClasses( int id )
        {
            // TODO
            return null;
        }

        override
        protected SchoolObject getObjectFromReader( MySqlDataReader reader )
        {
            // Create a Teacher object for the function to return.
            // If no teacher was found, the members of the object
            // will be uninitialized.
            Teacher teacher = new Teacher();

            // Read the reader. There should be only one row.
            while( reader.Read() ) {
                teacher.employeeNumber = reader[ "employeenumber" ].ToString();
                teacher.firstName = reader[ "teacherfname" ].ToString();
                teacher.lastName = reader[ "teacherlname" ].ToString();
                teacher.teacherId = reader.GetInt32( reader.GetOrdinal( "teacherid" ) );
                teacher.salary = reader.GetDecimal( reader.GetOrdinal( "salary" ) );
                teacher.hireDate = reader.GetDateTime( reader.GetOrdinal( "hiredate" ) );
            }

            // Get courses
            teacher.courses = getClasses( teacher.teacherId );

            // Return the teacher object.
            return teacher;
        }

        /// <summary>
        /// Returns a list of Teacher objects constructed with data for each teacher
        /// that matches the column name and value in the School database.
        /// </summary>
        /// <input>A string value that matches one of the column names in the teachers table.</input>
        /// <input>A string value to match against values in that column.</input>
        /// <returns>
        /// A list of Teacher objects with the teachers' information if one or more 
        /// teachers match the search criteria, otherwise an empty list. If either
        /// columnName or columnValue is null, an empty list is returned.
        /// </returns>
        /// <example>
        /// GET api/TeacherData/getTeachers/{columnName}/{columnValue}
        /// </example>
        /// <example>
        /// GET api/TeacherData/getTeachers/teacherfname/Caitlin
        /// </example>
        /// <example>
        /// GET api/TeacherData/getTeachers/
        /// </example>
        [HttpGet]
        [Route( "api/TeacherData/getTeachers/{columnName}/{columnValue}" )]
        public IEnumerable<Teacher> getTeachers( string columns, string conditions )
        {
            return (IEnumerable<Teacher>) find( "teachers", columns, conditions );
        }

        override
        protected IEnumerable<SchoolObject> getSearchResultsFromReader( MySqlDataReader reader )
        {
            // There may be more than one result.
            List<Teacher> teachers = new List<Teacher>();

            // At the same time, there may be none. If not, return the empty list.
            if( reader.HasRows ) {
                // Read the reader, one row at a time.
                while( reader.Read() ) {
                    Teacher teacher = new Teacher();

                    teacher.employeeNumber = reader[ "employeenumber" ].ToString();
                    teacher.firstName = reader[ "teacherfname" ].ToString();
                    teacher.lastName = reader[ "teacherlname" ].ToString();
                    teacher.teacherId = reader.GetInt32( reader.GetOrdinal( "teacherid" ) );
                    teacher.salary = reader.GetDecimal( reader.GetOrdinal( "salary" ) );
                    teacher.hireDate = reader.GetDateTime( reader.GetOrdinal( "hiredate" ) );

                    teachers.Add( teacher );
                }
            }

            // Return the list of teachers.
            return teachers;
        }
    }
}

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
            Teacher teacher = (Teacher) get( "teachers", "teacherid", id );
            ClassDataController contoller = new ClassDataController();
            IEnumerable<SchoolObject> schoolObjects = contoller.findClasses( "classes.teacherid = " + id );
            List<Class> classes = new List<Class>();
            foreach( SchoolObject obj in schoolObjects ) {
                classes.Add( (Class) obj );
            }
            teacher.courses = classes;
            return teacher;
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

            // Return the teacher object.
            return teacher;
        }

        /// <summary>
        /// Returns a list of Teacher objects constructed with data for each teacher
        /// that matches the given condition in the School database.
        /// </summary>
        /// <input>A string value use in the WHERE clause of the query.</input>
        /// <returns>
        /// A list of Teacher objects with the teachers' information if one or 
        /// more teachers match the search criteria, otherwise an empty list. 
        /// </returns>
        /// <example>
        /// GET api/TeacherData/findTeachers/{condition}
        /// </example>
        /// <example>
        /// GET api/TeacherData/findTeachers/teacherfname=Caitlin
        /// </example>
        [HttpGet]
        [Route( "api/TeacherData/findTeachers/{condition}" )]
        public IEnumerable<Teacher> findTeachers( string condition )
        {
            return (IEnumerable<Teacher>) find( "teachers", condition );
        }

        override
        protected IEnumerable<SchoolObject> getSearchResultsFromReader( MySqlDataReader reader )
        {
            // If there are results, create a list. At this point the resulst contain
            // all the columns, so we can resuse getListFromReader.
            if( reader.HasRows ) {
                return getListFromReader( reader );
            }

            // Return an empty list.
            return new List<Teacher>();
        }

        public int addTeacher( Teacher teacher )
        {
            return add( "teachers", teacher.getProperties() );
        }

    }
}

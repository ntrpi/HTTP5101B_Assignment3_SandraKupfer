﻿using HTTP5101Assignment3.Models;
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
                // the database since we did not alias them.
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
        [Route( "api/TeacherData/getTeacher/{id}" )]
        public Teacher getTeacher( int? id )
        {
            // This is to address an error that happens if I start the debugger while
            // I am looking at Show.cshtml. In a professional setting, there would
            // be some way to have this not show up in a customer-facing release.
            if( id == null ) {
                return new Teacher();
            }

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

            // Close the reader so we can send another query.
            results.Close();

            // Set a query that will get all the courses taught by this teacher.
            command.CommandText = "SELECT classname from classes WHERE teacherid = " + id;

            // Execute the command and save the results.
            results = command.ExecuteReader();

            // Create a list object to hold the courses.
            List<String> courses = new List<string>();

            // Read the results, one row at a time.
            while( results.Read() ) {

                // The results will have the same column names as
                // the database since we did not alias them.
                // Add the class name to the list of courses.
                courses.Add( results[ "classname" ].ToString() );
            }

            // Add the list of courses to the teacher object.
            teacher.courses = courses;

            // Close the connection.
            connection.Close();

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
        [Route( "api/TeacherData/getTeachers/{columnName}/{columnValue}" )]

        public IEnumerable<Teacher> getTeachers( string columnName, string columnValue )
        {
            // This is to address an error that happens if I start the debugger while
            // I am looking at Results.cshtml. In a professional setting, there would
            // be some way to have this not show up in a customer-facing release.
            if( columnName == null || columnValue == null ) {
                return new List<Teacher>();
            }

            // Create and open a connection to the databas e.
            MySqlConnection connection = schoolDb.accessDatabase();
            connection.Open();

            // Create and set a query for the database that will retrieve
            // the teachers' first and last names.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM teachers WHERE " + columnName + " = \"" + columnValue + "\"";

            // Create an object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // There may be more than one result.
            List<Teacher> teachers = new List<Teacher>();

            // At the same time, there may be none. If not, return the empty list.
            if( !results.HasRows ) {
                return teachers;
            }

            // Read the results, one row at a time.
            while( results.Read() ) {
                Teacher teacher = new Teacher();

                teacher.employeeNumber = results[ "employeenumber" ].ToString();
                teacher.firstName = results[ "teacherfname" ].ToString();
                teacher.lastName = results[ "teacherlname" ].ToString();
                teacher.teacherId = results.GetInt32( results.GetOrdinal( "teacherid" ) );
                teacher.salary = results.GetDecimal( results.GetOrdinal( "salary" ) );
                teacher.hireDate = results.GetDateTime( results.GetOrdinal( "hiredate" ) );

                teachers.Add( teacher );
            }

            // Close the connection.
            connection.Close();

            // Return the list of teachers.
            return teachers;
        }
    }
}

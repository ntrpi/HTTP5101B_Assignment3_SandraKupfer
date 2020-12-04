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
        private string tableName = "teachers";
        private string idColumnName = "teacherId";

        override
        protected string getTableName()
        {
            return tableName;
        }

        override
        protected string getIdColumnName()
        {
            return idColumnName;
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
            return (IEnumerable<Teacher>) list();
        }

        override
        protected IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader )
        {
            // Create a list object to hold the teacher names.
            List<Teacher> teachers = new List<Teacher>();

            // Validate reader.
            if( reader == null || !reader.HasRows ) {
                return teachers;
            }

            // Read the reader, one row at a time.
            while( reader.Read() ) {

                // Add the teacher to the list.
                teachers.Add( getTeacherFromReader( reader ) );
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
            Teacher teacher = (Teacher) get( id );
            ClassDataController contoller = new ClassDataController();
            IEnumerable<SchoolObject> schoolObjects = contoller.findClasses( "classes.teacherid = " + id );
            List<Class> classes = new List<Class>();
            foreach( SchoolObject obj in schoolObjects ) {
                classes.Add( (Class) obj );
            }
            teacher.courses = classes;
            return teacher;
        }

        private Teacher getTeacherFromReader( MySqlDataReader reader )
        {
            // Create a Teacher object for the function to return.
            // If no teacher was found, the members of the object
            // will be uninitialized.
            Teacher teacher = new Teacher();

            // Read the reader. There should be only one row.
            teacher.employeeNumber = reader[ "employeenumber" ].ToString();
            teacher.teacherFName = reader[ "teacherfname" ].ToString();
            teacher.teacherLName = reader[ "teacherlname" ].ToString();
            teacher.teacherId = reader.GetInt32( reader.GetOrdinal( "teacherid" ) );
            teacher.salary = reader.GetDecimal( reader.GetOrdinal( "salary" ) );
            teacher.hireDate = reader.GetDateTime( reader.GetOrdinal( "hiredate" ) );

            // Return the teacher object.
            return teacher;
        }

        override
        protected SchoolObject getObjectFromReader( MySqlDataReader reader )
        {
            // Validate reader.
            if( reader == null || !reader.HasRows ) {
                return null;
            }

            reader.Read();
            return getTeacherFromReader( reader );
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
            return (IEnumerable<Teacher>) find( condition );
        }

        public int addTeacher( Teacher teacher )
        {
            return add( teacher.getProperties() );
        }

        public int deleteTeacher( int id )
        {
            return delete( "teacherId=" + id );        
        }

    }
}

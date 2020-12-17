using HTTP5101Assignment3.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

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
        [EnableCors( origins: "*", methods: "*", headers: "*" )]
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
        [EnableCors( origins: "*", methods: "*", headers: "*" )]
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
            // There should be only one row.
            return new Teacher(
                reader.GetInt32( reader.GetOrdinal( "teacherid" ) ),
                reader[ "teacherfname" ].ToString(),
                reader[ "teacherlname" ].ToString(),
                reader[ "employeenumber" ].ToString(),
                reader[ reader.GetOrdinal( "hiredate" ) ].ToString(),
                reader.GetDecimal( reader.GetOrdinal( "salary" ) )
            );
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
        [EnableCors( origins: "*", methods: "*", headers: "*" )]
        public IEnumerable<Teacher> findTeachers( string condition )
        {
            return (IEnumerable<Teacher>) find( condition );
        }

        /// <summary>
        /// Add the Teacher object to the database.
        /// </summary>
        /// <param name="teacher">Teacher object to add to the database.</param>
        /// <returns>An integer value that is the result of SchoolObjectDataController
        /// adding the teacher to the database.</returns>
        /// <example>
        /// POST api/TeacherData/addTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	    "teacherFname":"Sandra",
        ///	    "teacherLname":"Kupfer",
        ///	    "employeeNumber":"T8374",
        ///	    "hireDate":"2020-10-10",
        ///	    "salary":"60.75"
        /// }
        /// </example>
        [HttpPost]
        [EnableCors( origins: "*", methods: "*", headers: "*" )]
        public int addTeacher( [FromBody] Teacher teacher )
        {
            return add( teacher.getProperties() );
        }

        /// <summary>
        /// Update the teacher with the properties from the Teacher object in the database.
        /// </summary>
        /// <param name="teacher">Teacher object containing the properties to update in 
        /// the database.</param>
        /// <returns>An integer value that is the result of SchoolObjectDataController
        /// updating the teacher in the database.</returns>
        /// <example>
        /// POST api/TeacherData/addTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	    "teacherFname":"Sandra",
        ///	    "teacherLname":"Kupfer",
        ///	    "employeeNumber":"T8374",
        ///	    "hireDate":"2020-10-10",
        ///	    "salary":"60.75"
        /// }
        /// </example>
        public int updateTeacher( Teacher teacher )
        {
            return update( teacher.getProperties() );
        }

        /// <summary>
        /// Delete a Teacher from the database if a teacher with that id exists. 
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>POST /api/TeacherData/deleteTeacher/5</example>
        [HttpPost]
        [EnableCors( origins: "*", methods: "*", headers: "*" )]
        public int deleteTeacher( int id )
        {
            return delete( "teacherId=" + id );        
        }

    }
}

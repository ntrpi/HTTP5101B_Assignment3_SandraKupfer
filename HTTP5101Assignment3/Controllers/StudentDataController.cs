using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using HTTP5101Assignment3.Models;

namespace HTTP5101Assignment3.Controllers
{
    public class StudentDataController : SchoolObjectDataController
    {
        private string tableName = "students";
        private string idColumnName = "studentId";

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
        /// Returns a list of students in the School database.
        /// </summary>
        /// <returns>
        /// A list of the students' first and last names.
        /// </returns>
        /// <example>
        /// GET api/StudentData/listStudents
        /// </example>
        [HttpGet]
        [Route( "api/StudentData/listStudents" )]
        public IEnumerable<Student> listStudents()
        {
            return (IEnumerable<Student>) list();
        }

        override
        protected IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader )
        {
            // Create a list object to hold the student names.
            List<Student> students = new List<Student>();

            // Validate reader.
            if( reader == null ) {
                return students;
            }

            // Read the reader, one row at a time.
            while( reader.Read() ) {

                // Add the student to the list.
                students.Add( getStudentFromReader( reader ) );
            }

            return students;
        }

        /// <summary>
        /// Returns a Student object constructed with data for the student
        /// with the given id in the School database. If the id is null or 
        /// the student not found, the object will be uninitialized
        /// </summary>
        /// <input>An integer value representing the student's ID.</input>
        /// <returns>
        /// A Student object with the student's information if a student 
        /// with the given id exists in the database, otherwise an uninitialized
        /// Student object..
        /// </returns>
        /// <example>
        /// GET api/StudentData/getStudent/{id}
        /// </example>
        /// <example>
        /// GET api/StudentData/getStudent
        /// </example>
        [HttpGet]
        [Route( "api/StudentData/getStudent/{id}" )]
        public Student getStudent( int? id )
        {
            Student student = (Student) get( id );
            ClassDataController contoller = new ClassDataController();
            IEnumerable<SchoolObject> schoolObjects = contoller.findClasses( "classes.studentid = " + id );
            List<Class> classes = new List<Class>();
            foreach( SchoolObject obj in schoolObjects ) {
                classes.Add( (Class) obj );
            }
            student.courses = classes;
            return student;
        }

        private Student getStudentFromReader( MySqlDataReader reader )
        {
            // Create a Student object for the function to return.
            // If no student was found, the members of the object
            // will be uninitialized.
            Student student = new Student();

            // Read the reader. There should be only one row.
            student.studentNumber = reader[ "studentNumber" ].ToString();
            student.studentFName = reader[ "studentfname" ].ToString();
            student.studentLName = reader[ "studentlname" ].ToString();
            student.studentId = reader.GetInt32( reader.GetOrdinal( "studentid" ) );

            // Return the student object.
            return student;
        }

        override
        protected SchoolObject getObjectFromReader( MySqlDataReader reader )
        {
            // Validate reader.
            if( reader == null || !reader.HasRows ) {
                return null;
            }

            reader.Read();
            return getStudentFromReader( reader );
        }

        /// <summary>
        /// Returns a list of Student objects constructed with data for each student
        /// that matches the given condition in the School database.
        /// </summary>
        /// <input>A string value use in the WHERE clause of the query.</input>
        /// <returns>
        /// A list of Student objects with the students' information if one or 
        /// more students match the search criteria, otherwise an empty list. 
        /// </returns>
        /// <example>
        /// GET api/StudentData/findStudents/{condition}
        /// </example>
        /// <example>
        /// GET api/StudentData/findStudents/studentfname=Caitlin
        /// </example>
        [HttpGet]
        [Route( "api/StudentData/findStudents/{condition}" )]
        public IEnumerable<Student> findStudents( string condition )
        {
            return (IEnumerable<Student>) find( condition );
        }

        public int addStudent( Student student )
        {
            return add( student.getProperties() );
        }

        public int deleteStudent( int id )
        {
            return delete( "studentId=" + id );
        }


    }
}

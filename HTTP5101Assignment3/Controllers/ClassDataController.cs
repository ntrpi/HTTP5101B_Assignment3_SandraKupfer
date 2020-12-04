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
    public class ClassDataController: SchoolObjectDataController
    {
        private string tableName = "classes";
        private string idColumnName = "classId";

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
        /// Returns a list of classes in the School database.
        /// </summary>
        /// <returns>
        /// A list of the classes' first and last names.
        /// </returns>
        /// <example>
        /// GET api/ClassData/listClasses
        /// </example>
        [HttpGet]
        [Route( "api/ClassData/listClasses" )]
        public IEnumerable<Class> listClasses()
        {
            return (IEnumerable<Class>) list();
        }

        override
        protected IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader )
        {
            // Create a list object to hold the class names.
            List<Class> classes = new List<Class>();

            // Validate reader.
            if( reader == null ) {
                return classes;
            }

            // Read the reader, one row at a time.
            while( reader.Read() ) {

                // Add the class to the list.
                classes.Add( getClassFromReader( reader ) );
            }

            return classes;
        }

        /// <summary>
        /// Returns a Class object constructed with data for the class
        /// with the given id in the School database. If the id is null or 
        /// the class not found, the object will be uninitialized
        /// </summary>
        /// <input>An integer value representing the class's ID.</input>
        /// <returns>
        /// A Class object with the class's information if a class 
        /// with the given id exists in the database, otherwise an uninitialized
        /// Class object..
        /// </returns>
        /// <example>
        /// GET api/ClassData/getClass/{id}
        /// </example>
        /// <example>
        /// GET api/ClassData/getClass
        /// </example>
        [HttpGet]
        [Route( "api/ClassData/getClass/{id}" )]
        public Class getClass( int? id )
        {
            Class course = (Class) get( id );
            ClassDataController contoller = new ClassDataController();
            IEnumerable<SchoolObject> schoolObjects = contoller.findClasses( "classes.classid = " + id );
            List<Class> classes = new List<Class>();
            foreach( SchoolObject obj in schoolObjects ) {
                classes.Add( (Class) obj );
            }
            return course;
        }

        private Class getClassFromReader( MySqlDataReader reader )
        {
            // Create a Class object for the function to return.
            // If no class was found, the members of the object
            // will be uninitialized.
            Class course = new Class();

            // Read the reader. There should be only one row.
            course.classCode = reader[ "classcode" ].ToString();
            course.className = reader[ "classname" ].ToString();
            course.classId= (int) reader[ "classId" ];
            course.startDate = reader.GetDateTime(reader.GetOrdinal( "startdate" ) );
            course.finishDate = reader.GetDateTime(reader.GetOrdinal( "finishdate" ) );

            // Return the class object.
            return course;
        }

        override
        protected SchoolObject getObjectFromReader( MySqlDataReader reader )
        {
            // Validate reader.
            if( reader == null || !reader.HasRows ) {
                return null;
            }

            reader.Read();
            return getClassFromReader( reader );
        }

        /// <summary>
        /// Returns a list of Class objects constructed with data for each class
        /// that matches the given condition in the School database.
        /// </summary>
        /// <input>A string value use in the WHERE clause of the query.</input>
        /// <returns>
        /// A list of Class objects with the classes' information if one or 
        /// more classes match the search criteria, otherwise an empty list. 
        /// </returns>
        /// <example>
        /// GET api/ClassData/findClasses/{condition}
        /// </example>
        /// <example>
        /// GET api/ClassData/findClasses/classfname=Caitlin
        /// </example>
        [HttpGet]
        [Route( "api/ClassData/findClasses/{condition}" )]
        public IEnumerable<Class> findClasses( string condition )
        {
            return (IEnumerable<Class>) find( condition );
        }

        public int addClass( Class course )
        {
            return add( course.getProperties() );
        }

        public int deleteClass( int id )
        {
            return delete( "classId=" + id );
        }


    }
}

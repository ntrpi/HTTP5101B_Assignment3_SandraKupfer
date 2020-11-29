using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HTTP5101Assignment3.Models;
using MySql.Data.MySqlClient;

namespace HTTP5101Assignment3.Controllers
{
    public class ClassDataController : SchoolObjectDataController
    {
        /// <summary>
        /// Returns a list of Class objects constructed with data for each class
        /// that matches condition in the School database.
        /// </summary>
        /// <input>A string value to use in the WHERE clause of the query.</input>
        /// <returns>
        /// A list of Class objects with the classes' information if one or 
        /// more classes match the search criteria, otherwise an empty list. 
        /// </returns>
        /// <example>
        /// GET api/ClassData/findClasses/{condition}
        /// </example>
        /// <example>
        /// GET api/ClassData/findClasses/teacherid=4
        /// </example>
        [HttpGet]
        [Route( "api/ClassData/findClasses/{condition}" )]
        public IEnumerable<Class> findClasses( string condition )
        {
            return (IEnumerable<Class>) find( "classes", condition );
        }

        /// <summary>
        /// Returns a list of classes in the School database.
        /// </summary>
        /// <returns>
        /// A list of the Class objects.
        /// </returns>
        /// <example>
        /// GET api/ClassData/listClasses
        /// </example>
        [HttpGet]
        [Route( "api/ClassData/listClasses" )]
        public IEnumerable<Class> listClasses()
        {
            return (IEnumerable<Class>) list( "classes" );
        }



        protected override IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader )
        {
            // Create a list object to hold the class names.
            List<Class> classes = new List<Class>();

            // Read the reader, one row at a time.
            while( reader.Read() ) {

                // Create a new Class object.
                Class classObject = new Class();

                // The reader will have the same column names as
                // the database since we did not alias them.
                // Put the data in the corresponding field of the Class
                // object.
                classObject.classId = reader.GetInt32( reader.GetOrdinal( "classid" ) );
                classObject.classCode = reader[ "classcode" ].ToString();
                classObject.className = reader[ "classname" ].ToString();
                classObject.startDate = reader.GetDateTime( reader.GetOrdinal( "startdate" ) );
                classObject.finishDate = reader.GetDateTime( reader.GetOrdinal( "finishdate" ) );
                classObject.classId = reader.GetInt32( reader.GetOrdinal( "classid" ) );

                // Add the class to the list.
                classes.Add( classObject );
            }

            return classes;
        }

        protected override SchoolObject getObjectFromReader( MySqlDataReader reader )
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<SchoolObject> getSearchResultsFromReader( MySqlDataReader reader )
        {
            // If there are results, create a list. At this point the resulst contain
            // all the columns, so we can resuse getListFromReader.
            if( reader.HasRows ) {
                return getListFromReader( reader );
            }

            // Return an empty list.
            return new List<Class>();
        }
    }
}

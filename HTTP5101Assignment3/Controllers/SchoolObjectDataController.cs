using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using HTTP5101Assignment3.Models;
using MySql.Data.MySqlClient;

namespace HTTP5101Assignment3.Controllers
{
    public abstract class SchoolObjectDataController : ApiController
    {
        // This object will handle the interactions with the
        // database.
        private static SchoolDbContext schoolDb = new SchoolDbContext();

        private static MySqlConnection connection;
        private static MySqlConnection getConnection()
        {
            if( connection == null ) {
                // Create and open a connection to the database.
                connection = schoolDb.accessDatabase();
            }

            if( connection.State == System.Data.ConnectionState.Closed ) {
                // Try to open the connection.
                try {
                    connection.Open();

                } catch( Exception e ) {

                    // If unable to connect, output the exception to the console 
                    // and return null.
                    Console.Write( e );
                }
            }
            return connection;
        }

        protected abstract string getTableName();

        protected abstract string getIdColumnName();

        protected abstract IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader );

        protected abstract SchoolObject getObjectFromReader( MySqlDataReader reader );

        protected abstract IEnumerable<SchoolObject> getSearchResultsFromReader( MySqlDataReader reader );


        private MySqlDataReader executeMySqlQuery( string query )
        {
            // Validate connection.
            if( getConnection() == null ) {

                // Return null if the connection was not successful.
                return null;
            }

            // Set the query.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = query;

            // Create an object to hold the reader of the command
            // and execute.
            MySqlDataReader reader = command.ExecuteReader();

            // Return the reader.
            return reader;
        }

        private int executeMySqlNonQuery( string nonQuery )
        {
            // Validate connection.
            if( getConnection() == null ) {

                // Return -1 if the connection was not successful.
                return -1;
            }

            // Set the query.
            MySqlCommand command = getConnection().CreateCommand();
            command.CommandText = nonQuery;

            // Create an object to hold the reader of the command
            // and execute.
            int result = command.ExecuteNonQuery();

            // Return the result.
            return result;
        }

        /// <summary>
        /// This function is intented to determine the highest ID for a table
        /// in the database for validation in the feature allowing the user to 
        /// find a specific school object by its ID.
        /// </summary>
        /// <returns>A positive integer that represents the highest ID in
        /// the database, or a 0 if unable to connect to the database.
        /// </returns>
        /// <example>
        /// GET api/SchoolObjectData/getHighestId
        /// </example>
        [HttpGet]
        public MaxId getHighestId()
        {
            // Create a query for the database that will retrieve
            // the highest id for the class that implemented this one.
            string query = "SELECT "
                + getIdColumnName()
                + " FROM "
                + getTableName() 
                + " ORDER BY "
                + getIdColumnName() 
                + " DESC LIMIT 1";

            // Create an object to hold the reader of the command
            // and execute.
            MySqlDataReader reader = executeMySqlQuery( query );

            // Validate the reader.
            if( reader == null ) {
                return new MaxId(0);
            }

            // Declare an int to hold the highest id. Initialize it to
            // 0.
            int maxId = 0;

            // Read the reader. There should be only one row.
            while( reader.Read() ) {
                maxId = reader.GetInt32( reader.GetOrdinal( getIdColumnName() ) );
            }

            // Close the connection.
            getConnection().Close();

            // Return the max teacher ID.
            return new MaxId( maxId );
        }

        // TODO: use this to validate input and/or results.
        public bool ifColumnExists( MySqlDataReader reader, string columnName )
        {
            for( int i = 0; i < reader.FieldCount; i++ ) {
                if( reader.GetName( i ).Equals( columnName, StringComparison.InvariantCultureIgnoreCase ) ) {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns a list of requested items in the School database or null if 
        /// the input is invalid or unable to connect to the database.
        /// </summary>
        /// <input>Name of the table from which to get the items.</input>
        /// <returns>
        /// A list of school objects or null.
        /// </returns>
        /// <example>
        /// GET api/SchoolObjectData/list
        /// </example>
        [HttpGet]
        [Route( "api/SchoolObjectData/list" )]
        protected IEnumerable<SchoolObject> list()
        {
            // Create a query for the database that will retrieve
            // the all the data from the given table.
            string query = "SELECT * FROM " + getTableName();

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = executeMySqlQuery( query );

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            IEnumerable<SchoolObject> list = getListFromReader( results );

            // Close the connection to the database.
            getConnection().Close();

            // Return the list.
            return list;
        }

        /// <summary>
        /// Returns a SchoolObject constructed with data for the item
        /// with the given id in the School database. If the id is null or 
        /// the item not found, null will be returned.
        /// </summary>
        /// <input>The name of the table from which to get the item.</input>
        /// <input>The name of the column that contains the id.</input>
        /// <input>An integer value representing the id of the item.</input>
        /// <returns>
        /// A SchoolObject initialized with the information from the database if 
        /// such and item was found, otherwise an null.
        /// </returns>
        /// <example>
        /// GET api/SchoolObjectData/get/{id}
        /// </example>
        [HttpGet]
        [Route( "api/SchoolObjectData/get/{id}" )]
        protected SchoolObject get( int? id )
        {
            // This is to address an error that happens if I start the debugger while
            // I am looking at Show.cshtml. In a professional setting, there would
            // be some way to have this not show up in a customer-facing release.
            if( id == null ) {
                return null;
            }

            // Create a query for the database that will retrieve
            // the all the data from the given table.
            string query = "SELECT * FROM "
                + getTableName() 
                + " WHERE " 
                + getIdColumnName() 
                + " = " + id + ";";

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = executeMySqlQuery( query );

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            SchoolObject schoolObject = getObjectFromReader( results );

            // Close the connection to the database.
            getConnection().Close();

            // Return the list.
            return schoolObject;
        }

        /// <summary>
        /// Returns a list of SchoolObjects for each item that matches the
        /// that matches the condition string in the School database.
        /// </summary>
        /// <input>The name of the table.</input>
        /// <input>A string value to use in the WHERE clause.</input>
        /// <returns>
        /// A list of SchoolObjects if one or more items match the search criteria, 
        /// otherwise an empty list. If either table is null, null is returned.
        /// </returns>
        /// <example>
        /// GET api/SchoolObjectData/find/{table}/{condition}
        /// </example>
        /// <example>
        /// GET api/SchoolObjectData/find/teachers/teacherid=4
        /// </example>
        /// <example>
        /// </example>
        [HttpGet]
        [Route( "api/SchoolObjectData/find/{condition}" )]
        protected IEnumerable<SchoolObject> find( string condition )
        {
            // Validate the condition.
            if( condition == null ) {
                condition = "1";
            }

            // Create a query for the database that will retrieve
            // the all the data from the given table.
            string query = "SELECT * FROM " 
                + getTableName()
                + " WHERE "
                + condition
                + ";";

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = executeMySqlQuery( query );

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            IEnumerable<SchoolObject> list = getSearchResultsFromReader( results );

            // Close the connection to the database.
            getConnection().Close();

            // Return the list.
            return list;
        }

        // TODO: comments
        protected int add( OrderedDictionary properties )
        {
            // Create a query for the database that will insert
            // all the values for the given table row.
            // Construct the query.
            StringBuilder query = new StringBuilder();
            query.Append( "INSERT INTO " ).Append( getTableName() ).Append( "( " );

            // Get the property names and values.
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            bool isFirst = true;
            foreach( DictionaryEntry entry in properties ) {
                if( isFirst ) {
                    isFirst = false;
                    continue;
                }
                columns.Append( entry.Key ).Append( ", " );
                values.Append( '"' ).Append( entry.Value ).Append( "\", " );
            }

            // Shave the trailing comma and space.
            columns.Length -= 2;
            values.Length -= 2;

            // Add to the query.
            query.Append( columns.ToString() ).Append( ") VALUES ( " ).Append( values.ToString() ).Append( " );" );

            // Create a and object to hold the results of the command
            // and execute.
            int result = executeMySqlNonQuery( query.ToString() );

            // Close the connection to the database.
            getConnection().Close();

            return result;
        }


        public int delete( string condition )
        {
            // Create a query for the database that will retrieve
            // the all the data from the given table.
            string query = "DELETE FROM " 
                + getTableName() 
                + " WHERE " 
                + condition;

            // Create a and object to hold the results of the command
            // and execute.
            int result = executeMySqlNonQuery( query );

            // Close the connection to the database.
            getConnection().Close();

            // Return the list.
            return result;
        }
    }
}

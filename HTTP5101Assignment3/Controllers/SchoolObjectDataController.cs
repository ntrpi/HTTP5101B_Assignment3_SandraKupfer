﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HTTP5101Assignment3.Models;
using MySql.Data.MySqlClient;

namespace HTTP5101Assignment3.Controllers
{
    public abstract class SchoolObjectDataController : ApiController
    {
        // This object will handle the interactions with the
        // database.
        private SchoolDbContext schoolDb = new SchoolDbContext();

        /// <summary>
        /// This is a private utility function that opens a connection to 
        /// the database to save on typing.
        /// </summary>
        /// <returns>Open MySqlConnection if creation and opening is successful,
        /// null otherwise</returns>
        protected MySqlConnection getConnection()
        {
            // Create and open a connection to the database.
            MySqlConnection connection = schoolDb.accessDatabase();

            // Try to open the connection.
            try {
                connection.Open();
                return connection;

            } catch( Exception e ) {

                // If unable to connect, output the exception to the console 
                // and return null.
                Console.Write( e );
                return null;
            }
        }

        protected abstract IEnumerable<SchoolObject> getListFromReader( MySqlDataReader reader );

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
        protected IEnumerable<SchoolObject> list( string table )
        {
            // Validate the input.
            if( table == null ) {
                return null;
            }

            // Create and open a connection to the database.
            MySqlConnection connection = getConnection();
            if( connection == null ) {

                // Return null if the connection was not successful.
                return null;
            }

            // Create and set a query for the database that will retrieve
            // the all the data from the given table.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM @table";
            command.Parameters.AddWithValue( "@table", table );
            command.Prepare();

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            IEnumerable<SchoolObject> list = getListFromReader( results );

            // Close the connection to the database.
            connection.Close();

            // Return the list.
            return list;
        }

        protected abstract SchoolObject getObjectFromReader( MySqlDataReader reader );

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
        protected SchoolObject get( string table, string column, int? id )
        {
            // This is to address an error that happens if I start the debugger while
            // I am looking at Show.cshtml. In a professional setting, there would
            // be some way to have this not show up in a customer-facing release.
            if( id == null ) {
                return null;
            }

            // Validate the input.
            if( table == null || column == null ) {
                return null;
            }

            // Create and open a connection to the database.
            MySqlConnection connection = getConnection();
            if( connection == null ) {

                // Return null if the connection was not successful.
                return null;
            }

            // Create and set a query for the database that will retrieve
            // the all the data from the given table.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM @table WHERE @column = @id";
            command.Parameters.AddWithValue( "@table", table );
            command.Parameters.AddWithValue( "@column", column );
            command.Parameters.AddWithValue( "@id", id );
            command.Prepare();

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            SchoolObject schoolObject = getObjectFromReader( results );

            // Close the connection to the database.
            connection.Close();

            // Return the list.
            return schoolObject;
        }

        protected abstract IEnumerable<SchoolObject> getSearchResultsFromReader( MySqlDataReader reader );

        /// <summary>
        /// Returns a list of SchoolObjects for each item that matches the
        /// that matches the condition string in the School database.
        /// </summary>
        /// <input>The name of the table.</input>
        /// <input>A string value that contains one or more of the column names 
        /// in the given table.</input>
        /// <input>A string value to match against values in those columns.</input>
        /// <returns>
        /// A list of SchoolObjects if one or more items match the search criteria, 
        /// otherwise an empty list. If either
        /// columnName or columnValue is null, an empty list is returned.
        /// </returns>
        /// <example>
        /// GET api/SchoolObjectData/find/{table}/{columns}/{condition}
        /// </example>
        /// <example>
        /// </example>
        /// <example>
        /// </example>
        [HttpGet]
        [Route( "api/SchoolObjectData/find/{table}/{columns}/{condition}" )]
        protected IEnumerable<SchoolObject> find( string table, string columns, string condition )
        {
            // Create and open a connection to the database.
            MySqlConnection connection = getConnection();
            if( connection == null ) {

                // Return an null if the connection was not successful.
                return null;
            }

            // Create and set a query for the database that will retrieve
            // the all the data from the given table.
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT @columns FROM @table WHERE @condition";
            command.Parameters.AddWithValue( "@table", table );
            command.Parameters.AddWithValue( "@columns", columns );
            command.Parameters.AddWithValue( "@condition", condition );
            command.Prepare();

            // Create a and object to hold the results of the command
            // and execute.
            MySqlDataReader results = command.ExecuteReader();

            // Call the function implemented by the specific school object
            // create a list of the correct object type from the data
            // provided.
            IEnumerable<SchoolObject> list = getSearchResultsFromReader( results );

            // Close the connection to the database.
            connection.Close();

            // Return the list.
            return list;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;

namespace HTTP5101Assignment3.Models
{
    // Much of this class was copied from 
    // https://github.com/christinebittle/BlogProject_1/blob/master/BlogProject/Models/BlogDbContext.cs
    // as cloned on 2020/11/04.
    public class SchoolDbContext
    {
        // These properties and accessors contain the information required to connect to 
        // the local database.
        // *My first instinct is to remove the accessors, as they are redundant
        // since none of these properties need to be accessed outside of this
        // class. Also, because they are hard-coded, the properties themselves
        // aren't required as the connection string can be hard-coded as a private
        // property with an accessor. However, I believe that these accessors are
        // here to demonstrate scalability, so I'm leaving them in.
        private static string user { get { return "root"; } }
        private static string password { get { return "root"; } }
        private static string database { get { return "School"; } }
        private static string server { get { return "localhost"; } }
        private static string port { get { return "3306"; } }

        // Generate the string from the properties above that will be used
        // to connect to the database.
        protected static string connectionString 
        {
            get 
            {
                return "server = " + server
                    + "; user = " + user
                    + "; database = " + database
                    + "; port = " + port
                    + "; password = " + password;
            }
        }

        /// <summary>
        /// Create and return a connection to School database.
        /// </summary>
        /// <example>
        /// SchoolDbContext school = new SchoolDbContext();
        /// MySqlConnection connection = school.accessDatabase();
        /// </example>
        /// A MySqlConnection object that is connected to the School database.
        /// 
        public MySqlConnection accessDatabase()
        {
            // Create and return a MySqlConnection object that is connected to
            // the School database.
            return new MySqlConnection( connectionString );
        }
    }
}
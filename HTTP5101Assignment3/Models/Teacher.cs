using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Specialized;

namespace HTTP5101Assignment3.Models
{
    // This class mirrors the structure of the corresponding
    // teachers table in the School database. 
    public class Teacher : SchoolObject
    {
        public int teacherId;
        public string employeeNumber;
        public DateTime hireDate;
        public decimal salary;
        public string teacherFName;
        public string teacherLName;

        public IEnumerable<Class> courses;

        public OrderedDictionary getProperties()
        {
            OrderedDictionary properties = new OrderedDictionary();
            properties.Add( "teacherId", teacherId );
            properties.Add( "employeeNumber", employeeNumber );
            properties.Add( "hireDate", hireDate.ToString( "yyyy-MM-dd HH-mm-ss" ) );
            properties.Add( "salary", salary );
            properties.Add( "teacherFName", teacherFName );
            properties.Add( "teacherLName", teacherLName );
            return properties;
        }

        public IEnumerable<string> getPropertyList( string property )
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> getPropertyNames()
        {
            throw new NotImplementedException();
        }

    }
}
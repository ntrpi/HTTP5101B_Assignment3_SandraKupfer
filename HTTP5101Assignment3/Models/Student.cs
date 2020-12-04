using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HTTP5101Assignment3.Models
{
    public class Student : SchoolObject
    {
        public int studentId;
        public string studentFName;
        public string studentLName;
        public string studentNumber;
        public DateTime enrollDate;

        public IEnumerable<Class> courses;

        public OrderedDictionary getProperties()
        {
            OrderedDictionary properties = new OrderedDictionary();
            properties.Add( "studentId", studentId );
            properties.Add( "studentFName", studentFName );
            properties.Add( "studentLName", studentLName );
            properties.Add( "studentNumber", studentNumber );
            properties.Add( "enrolDate", enrollDate.ToString( "yyyy-MM-dd HH-mm-ss" ) );
            return properties;
        }

        public IEnumerable<string> getPropertyNames()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> getPropertyList( string property )
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HTTP5101Assignment3.Models
{
    public class Class : SchoolObject
    {
        public int classId;
        public string classCode;
        public int teacherId;
        public DateTime startDate;
        public DateTime finishDate;
        public string className;

        public OrderedDictionary getProperties()
        {
            OrderedDictionary properties = new OrderedDictionary();
            properties.Add( "classId", classId );
            properties.Add( "classCode", classCode );
            properties.Add( "teacherId", teacherId );
            properties.Add( "startDate", startDate.ToString( "yyyy-MM-dd HH-mm-ss" ) );
            properties.Add( "finishDate", finishDate.ToString( "yyyy-MM-dd HH-mm-ss" ) );
            properties.Add( "className", className );
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
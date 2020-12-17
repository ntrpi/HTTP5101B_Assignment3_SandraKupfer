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

        public Class( int classId, string className, string classCode, int teacherId, string startDate, string finishDate ) : this( className, classCode, teacherId, startDate, finishDate )
        {
            this.classId = classId;
        }

        public Class( string className, string classCode, int teacherId, string startDate, string finishDate )
        {
            this.className = className;
            this.classCode = classCode;
            this.teacherId = teacherId;
            if( startDate != null && !startDate.Equals( "" ) ) {
                this.startDate = Convert.ToDateTime( startDate ); // "dd/mm/yyyy"
            }
            if( finishDate != null && !finishDate.Equals( "" ) ) {
                this.finishDate = Convert.ToDateTime( finishDate ); // "dd/mm/yyyy"
            }
        }

        public string getPropertyError()
        {
            if( className == null || className.Length == 0 ) {
                return "class name";

            } else if( classCode == null || classCode.Length == 0 ) {
                return "class code";

            } else if( startDate == null ) {
                return "start date";

            } else if( finishDate == null ) {
                return "finish date";
            }
            return null;
        }

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
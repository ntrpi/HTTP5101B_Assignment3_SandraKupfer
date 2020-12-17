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

        public Student( int studentId, string studentFName, string studentLName, string studentNumber, string enrollDate ) : this( studentFName, studentLName, studentNumber, enrollDate )
        {
            this.studentId = studentId;
        }

        public Student( string studentFName, string studentLName, string studentNumber, string enrollDate )
        {
            this.studentFName = studentFName;
            this.studentLName = studentLName;
            this.studentNumber = studentNumber;
            if( enrollDate != null && !enrollDate.Equals( "" ) ) {
                this.enrollDate = Convert.ToDateTime( enrollDate ); // "dd/mm/yyyy"
            }
        }

        public string getPropertyError()
        {
            if( studentFName == null || studentFName.Length == 0 ) {
                return "first name";

            } else if( studentLName == null || studentLName.Length == 0 ) {
                return "last name";

            } else if( studentNumber == null || studentNumber.Length == 0 ) {
                return "student number";

            } else if( enrollDate == null ) {
                return "enroll date";
            }
            return null;
        }

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
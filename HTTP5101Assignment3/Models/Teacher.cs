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

        public Teacher( int teacherId, string teacherFName, string teacherLName, string employeeNumber, string hireDate, decimal salary ) : this( teacherFName, teacherLName, employeeNumber, hireDate, salary )
        {
            this.teacherId = teacherId;
        }

        public Teacher( string teacherFName, string teacherLName, string employeeNumber, string hireDate, decimal salary )
        {
            this.teacherFName = teacherFName;
            this.teacherLName = teacherLName;
            this.employeeNumber = employeeNumber;
            if( hireDate != null && !hireDate.Equals( "" ) ) {
                this.hireDate = Convert.ToDateTime( hireDate ); // "dd/mm/yyyy"
            }
            this.salary = salary;
        }


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

        public string getPropertyError()
        {
            if( teacherFName == null || teacherFName.Length == 0 ) {
                return "first name";

            } else if( teacherLName == null || teacherLName.Length == 0 ) {
                return "last name";

            } else if( employeeNumber == null || employeeNumber.Length == 0 ) {
                return "employee number";

            } else if( hireDate == null ) {
                return "hire date";
            }
            return null;
        }
    }
}
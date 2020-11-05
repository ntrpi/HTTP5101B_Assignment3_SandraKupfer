using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HTTP5101Assignment3.Models
{
    public class Teacher
    {
        int employeeNumber;
        HireDate hireDate;
        decimal salary;
        string firstName;
        string lastName;
        int teacherId;



    }

    class HireDate
    {
        private int day;
        private int month;
        private int year;

        private StringBuilder stringBuilder;

        public HireDate( int day, int month, int year )
        {
            this.day = day;
            this.month = month;
            this.year = year;

            stringBuilder = new StringBuilder( 10 );
        }

        public string getDateString()
        {
            stringBuilder.Clear();
            stringBuilder.Append( day.ToString() )
                .Append( '-' )
                .Append( month.ToString() )
                .Append( '-' )
                .Append( year.ToString() );
            return stringBuilder.ToString();
        }

        
    }
}
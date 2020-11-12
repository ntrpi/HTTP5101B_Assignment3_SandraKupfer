using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HTTP5101Assignment3.Models
{
    // This class mirrors the structure of the corresponding
    // teachers table in the School database. 
    public class Teacher
    {
        public string employeeNumber;
        public DateTime hireDate;
        public decimal salary;
        public string firstName;
        public string lastName;
        public int teacherId;

        public IEnumerable<String> courses;
    }
}
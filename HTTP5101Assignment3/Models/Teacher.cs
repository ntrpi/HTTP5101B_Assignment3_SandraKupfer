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
        public string employeeNumber;
        public DateTime hireDate;
        public decimal salary;
        public string firstName;
        public string lastName;
        public int teacherId;

        public IEnumerable<Class> courses;
        public OrderedDictionary getProperties()
        {
            OrderedDictionary properties = new OrderedDictionary();


            return properties;
        }

        public IEnumerable<String> getPropertyList()
        {
            List<String> classes = new List<string>();

            return classes;
        }

        public class MaxId
        {
            public Int32 maxId;

            public MaxId( int maxId )
            {
                this.maxId = maxId;
            }
        }
    }
}
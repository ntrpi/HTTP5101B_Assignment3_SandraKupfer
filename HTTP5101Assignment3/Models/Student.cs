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
        public DateTime enrolDate;

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

    }
}
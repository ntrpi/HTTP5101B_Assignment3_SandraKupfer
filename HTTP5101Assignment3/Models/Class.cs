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


            return properties;
        }

        public IEnumerable<String> getPropertyList()
        {
            List<String> students = new List<string>();

            return students;
        }

    }
}
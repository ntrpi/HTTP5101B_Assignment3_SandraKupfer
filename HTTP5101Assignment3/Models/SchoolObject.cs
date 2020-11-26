using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HTTP5101Assignment3.Models
{
    public interface SchoolObject
    {
        OrderedDictionary getProperties();
        IEnumerable<String> getPropertyList();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HTTP5101Assignment3.Models
{
    public interface SchoolObject
    {
        // Return the SchoolObject's properties as an ordered set of key/value
        // pairs where both the keys and values are strings. The set will only
        // contain the pairs where the value is a single property, as opposed
        // to a collection or object of some kind.
        // It is expected that the id is the first key/value pair.
        OrderedDictionary getProperties();

        // Return an ordered collection of strings representing the names of 
        // properties that are collections.
        IEnumerable<String> getPropertyNames();

        // Return an ordered collection of strings that represent the values
        // of a property that is a collection.
        IEnumerable<String> getPropertyList( string property );

        // Validate the properties and return a viewable string to indicate which
        // property is invalid, if any.
        string getPropertyError();
    }
}
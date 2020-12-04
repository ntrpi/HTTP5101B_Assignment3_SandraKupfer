using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5101Assignment3.Models
{
    public class AddError : ErrorMessage
    {
        public string schoolObjectName;
        public string missingProperty;

        public AddError( string schoolObjectName, string missingProperty )
        {
            this.schoolObjectName = schoolObjectName;
            this.missingProperty = missingProperty;
        }

        public string getErrorMessage()
        {
            return "Error: Unable to add new " + schoolObjectName + ": "
                + missingProperty
                + " is missing or invalid.";
        }
    }
}
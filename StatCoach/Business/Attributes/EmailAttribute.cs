using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatCoach.Business.Attributes
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute() : base ("^[a-z0z9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")
        {}
    }
}
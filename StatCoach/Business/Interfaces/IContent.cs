using StatCoach.Business.Enums;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatCoach.Business.Interfaces
{
    public interface IContent
    {
        Guid Id {get; set;}
        int? CreatedByUserId { get; set; }
        ContentType Type {get; set;}
        IEnumerable<ContentRight> ContentRights { get; set;} 
    }
}
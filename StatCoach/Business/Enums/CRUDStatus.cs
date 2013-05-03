using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatCoach.Business.Enums
{
    public enum CRUDStatus
    {
        Success = 0,
        Fail = 1,
        Duplicate = 2,
        UserError = 3,
        RoleError = 4,
        EntityNotFound = 5,
        AccessRightsError = 6
    }
}
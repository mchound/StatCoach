//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StatCoach.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Team
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CreatedByUserId { get; set; }
        public string RootRoleName { get; set; }
    
        public virtual User User { get; set; }
    }
}
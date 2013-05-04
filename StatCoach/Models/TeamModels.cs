using StatCoach.Business.Enums;
using StatCoach.Business.Interfaces;
using StatCoach.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatCoach.Models
{
    public class TeamModel : IContent
    {
        [Required(ErrorMessage = "Lagnamn kan ej vara tomt")]
        [Display(Name = "Lagnamn")]
        public string Name { get; set; }

        public int? CreatedByUserId { get; set; }

        public Guid Id { get; set; }

        public ContentType Type { get; set; }

        public IEnumerable<ContentRight> ContentRights { get; set; } 
        
    }
}
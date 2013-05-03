using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatCoach.Models
{
    public class TeamModel
    {
        [Required(ErrorMessage = "Lagnamn kan ej vara tomt")]
        [Display(Name = "Lagnamn")]
        public string Name { get; set; }

        public int? CreatedByUserId { get; set; }

        public Guid Id { get; set; }
    }
}
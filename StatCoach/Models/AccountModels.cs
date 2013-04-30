using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatCoach.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage="Måste ange E-Post")]
        [EmailAddress(ErrorMessage="Felaktig E-Post angiven")]
        [Display(Name = "E-Post")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Måste ange namn")]        
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "Lösenordet måste vara minst {2} bokstäver långt", MinimumLength=6)]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Lösenordet och det bekräftade lsöenordet matchar inte")]
        [Display(Name = "Bekräfta lösenord")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }        
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "E-Post måste anges")]
        [Display(Name = "E-Post")]
        public string Email { get; set; }

        [Required(ErrorMessage="Lösenord måste anges")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
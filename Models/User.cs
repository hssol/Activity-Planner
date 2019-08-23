using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class User
    {
    [Key]
    public int UserId {get;set;}
    /////////////////////////////////
    [Required]
    [MinLength(2)]
    public string FirstName {get;set;}
    /////////////////////////////////
    [Required]
    [MinLength(2)]
    public string LastName {get;set;}
    /////////////////////////////////
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    /////////////////////////////////
    [DataType(DataType.Password)]
    [Required]
    // [MinLength(8, ErrorMessage="Password must be 8 characters or longer")]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[@$!%*#?&])[A-Za-z0-9@$!%*#?&]{8,}$",ErrorMessage="Password must contain atleast 1 number, 1 letter, and 1 special character.")]
    public string Password {get;set;}
    /////////////////////////////////
    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public List<Association> OccurancesToAttend {get;set;}
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
    }
    public class LoginUser
    {
    // No other fields!
    public string Email {get; set;}
    public string Password { get; set; }
    }

}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Occurance
    {
        
        [Key]
        public int OccuranceId {get;set;}
        ////////////////////
        [Required]
        public string Title {get;set;}

        [FutureDate(ErrorMessage="Date should be in the future.")]
        public DateTime Date {get;set;} 

        [Range(0,9999999999999999)]
        public int Duration {get;set;}
        public string DurationType {get;set;}
        
        [Required]
        public string Description {get;set;}
        ////////////////////
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        ////////////////////
        public int CreatorId {get;set;}
        public User Creator {get;set;}
        public List<Association> Attendees {get;set;}
    }
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (DateTime)value > DateTime.Now;
        }
    }
}
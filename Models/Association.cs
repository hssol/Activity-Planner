using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Association
    {
        [Key]
        public int AssociationId {get;set;}
        ////////////////////
        public int UserId {get;set;}
        ////////////////////
        public int OccuranceId {get;set;}
        ////////////////////
        public User User {get;set;}
        public Occurance Occurance {get;set;}
    }
}
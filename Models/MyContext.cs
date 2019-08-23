using Microsoft.EntityFrameworkCore;
 
namespace BeltExam.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Occurance> Occurances {get;set;}
        public DbSet<Association> Associations {get;set;}
    }
}
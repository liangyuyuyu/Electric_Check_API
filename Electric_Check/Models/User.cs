using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Electric_Check.Models
{
    public class User
    {
        [Key]
        public string Account { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }

    public class Electric_CheckContext : DbContext
    {
        public Electric_CheckContext() { }
        public DbSet<User> Users { get; set; }
    }
}
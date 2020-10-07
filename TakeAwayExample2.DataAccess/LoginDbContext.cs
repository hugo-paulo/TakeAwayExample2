using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TakeAwayExample2.Models.ViewModels;

namespace TakeAwayExample2.DataAccess
{
    //Note because this is context for indentiy it must inherit from IdentityDbContext not DB context
    public class LoginDbContext: IdentityDbContext 
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options)
            : base(options)
        {
        }

        //We putting the User class here because we want EF to create the table 
        //if we had this table in DB we place it in the applicationDb context
        public DbSet<User> User { get; set; }

        //We need this context for asp identity, it is seperate so that EF doesnt affect the tables already in the DB
    }
}

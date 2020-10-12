using System;
using System.Collections.Generic;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;
using TakeAwayExample2.Models.ViewModels;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly LoginDbContext _ctx;

        public UserRepository(LoginDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public void LoginGracePeriod(User userLogin)
        {
            //Means lockout period not elapsed (thus user can still login)
            //Note! lockoutEnd can be used for stopping user from login from enting 3 wrong entries
            //userLogin cant be null (the field is bit either 1 or 0)
            //The lockoutEnabled means that the feature is enabled,if its not enabled lockout end is not used
            if (userLogin.LockoutEnabled && (userLogin.LockoutEnd == null || DateTime.Now < userLogin.LockoutEnd))
            {
                userLogin.LockoutEnd = DateTime.Now;
            }
            else
            {
                userLogin.LockoutEnd = DateTime.Now.AddYears(100);
                //? Do I set lockoutEnabled to 1 (true) or omitt it because the condition implies it would already be enabled ?
            }

            _context.SaveChanges();
        }
    }
}

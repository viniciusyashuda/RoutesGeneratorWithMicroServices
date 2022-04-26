using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAPI.Model;

namespace UserAPI.Data
{
    public class UserAPIContext : DbContext
    {
        public UserAPIContext (DbContextOptions<UserAPIContext> options)
            : base(options)
        {
        }

        public DbSet<UserAPI.Model.User> User { get; set; }
    }
}

using JwtTokenExample.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtTokenExample.DBContext
{
    public class UserDb : DbContext
    {
        public UserDb(DbContextOptions<UserDb> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
    }
}

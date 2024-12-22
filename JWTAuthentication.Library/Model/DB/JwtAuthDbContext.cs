using Microsoft.EntityFrameworkCore;
namespace JWTAuthentication.Library.Model.DB
{
    public class JwtAuthDbContext : DbContext
    {
        public JwtAuthDbContext(DbContextOptions<JwtAuthDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}

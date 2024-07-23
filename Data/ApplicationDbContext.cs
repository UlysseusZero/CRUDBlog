using Microsoft.EntityFrameworkCore;
using FortressTrialTask_JanJeffersonLam.Models;

namespace FortressTrialTask_JanJeffersonLam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<BlogPost> Posts { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using SafeVaultApp.Models;
using System.Collections.Generic;

namespace SafeVaultApp.DbModels
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }

}

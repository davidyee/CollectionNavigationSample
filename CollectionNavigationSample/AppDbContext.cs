using System;
using CollectionNavigationSample.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionNavigationSample
{
    public class AppDbContext : DbContext
    {
        public DbSet<Department> Department { get; set; }
        public DbSet<Employer> Employer { get; set; }
        public DbSet<Employee> Employee { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

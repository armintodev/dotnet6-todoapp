using dotnet6_training.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace dotnet6_training.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Todo> Todos { get; set; }
    }
}
using System;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Foresythe
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(b => b.Books)
                .WithMany(a => a.Authors);

            modelBuilder.Entity<Book>()
               .HasMany(a => a.Authors)
               .WithMany(b => b.Books);

            modelBuilder.Entity<Book>()
                .HasIndex(i => i.ISBN)
                .IsUnique(true);

            modelBuilder.Entity<Book>()
                .Property(e => e.Edition)
                .HasDefaultValue("1st");
        }
    }
}

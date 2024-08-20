using BookStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class BookStoreContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.Name).HasMaxLength(50);
                entity.Property(a => a.Nationality).HasMaxLength(15);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorId);
                entity.Property(b => b.Title).HasMaxLength(50);
                entity.Property(b => b.Genre).HasMaxLength(15);
            });
        }
    }
}
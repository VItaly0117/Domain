using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace DAL.Context;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Book>(entity =>
        {

            entity.HasKey(b => b.Id);
            
            entity.Property(b => b.Title)
                .IsRequired() 
                .HasMaxLength(200); 
            entity.Property(b => b.Author)
                .HasMaxLength(150);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.HasMany(u => u.Books) 
                .WithOne(b => b.User) 
                .HasForeignKey(b => b.UserId) 
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
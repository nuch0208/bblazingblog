using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingBlog.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set;}
        public DbSet<User> Users { get; set;}
        public DbSet<BlogPost> BlogPosts { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine);
#endif
        }

        // public override Task<int>SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     foreach(var entry in ChangeTracker.Entries())
        //     {
        //         if(entry.State == EntityState.Modified)
        //         {
        //             entry.Entity
        //         }
        //         else()
        //         {

        //         }
        //     }
        //     return base.SaveChangesAsync(cancellationToken);
        // } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Email = "abhayprince@outlook.com",
                        FirstName = "Abhay",
                        LastName = "Prince",
                        Salt = "12345",
                        Hash = "dfhkhsdskdfffgfjgfjgkdfhjgkdf/="
                    }
                );
        }
    }
}
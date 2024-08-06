using System;
using BlogAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class BlogAPIContext : IdentityDbContext<BlogUser>
    {
        public BlogAPIContext(DbContextOptions<BlogAPIContext> options)
            : base(options)
        {
        }


        public DbSet<BlogAPI.Model.BlogUser> blogUsers { get; set; }

        public DbSet<BlogAPI.Model.Post> Posts { get; set; }

        public DbSet<BlogAPI.Model.Comment> Comments { get; set; }

        public DbSet<BlogAPI.Model.Rating> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>().
                HasOne(x => x.ParentComment).
                WithMany(c=> c.Replies).
                HasForeignKey(x=> x.ParentCommentId).
                OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rating>().
                HasOne(c=> c.Comment).
                WithMany(r=> r.Ratings).
                HasForeignKey(c => c.CommentId ).
                OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>().
                HasOne(p => p.Post).
                WithMany(r=> r.Ratings).
                HasForeignKey(p=> p.PostId).
                OnDelete(DeleteBehavior.Restrict);
        }
    }
}

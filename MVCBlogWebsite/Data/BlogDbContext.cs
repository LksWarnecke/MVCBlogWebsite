using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Data
{
	public class BlogDbContext : DbContext
	{
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        // create two base tables -> BlogPosts and Tags in Database
        //need a third table because it is a many-to-many relationship
        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogPostLike> BlogPostLike { get; set; }

        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}

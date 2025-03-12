using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Data;
using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _blogDbContext.AddAsync(blogPost);
            await _blogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await _blogDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                _blogDbContext.BlogPosts.Remove(existingBlog);
                await _blogDbContext.SaveChangesAsync();

                return existingBlog;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            //get first blogPost with matching Id and include Tag information
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await _blogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await _blogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }
    }
}

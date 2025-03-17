using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Data;
using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public BlogPostCommentRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _blogDbContext.BlogPostComment.AddAsync(blogPostComment);
            await _blogDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdIAsync(Guid blogPostId)
        {
            //returning comments for the specific blog post, getting blogpost by id
            return await _blogDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}

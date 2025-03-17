using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdIAsync(Guid blogPostId);
    }
}

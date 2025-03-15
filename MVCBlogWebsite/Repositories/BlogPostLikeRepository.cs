
using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Data;
using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Repositories
{
	public class BlogPostLikeRepository : IBlogPostLikeRepository
	{
		private readonly BlogDbContext _blogDbContext;

		public BlogPostLikeRepository(BlogDbContext blogDbContext)
        {
			_blogDbContext = blogDbContext;
		}

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
			await _blogDbContext.BlogPostLike.AddAsync(blogPostLike);
			await _blogDbContext.SaveChangesAsync();
			return blogPostLike;
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
		{
			//get total number of likes of specific blogpost and return the number (of likes)
			return await _blogDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
		}
	}
}

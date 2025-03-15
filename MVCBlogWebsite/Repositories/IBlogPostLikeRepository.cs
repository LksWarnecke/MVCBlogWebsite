using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Repositories
{
	public interface IBlogPostLikeRepository
	{
		Task<int> GetTotalLikes(Guid blogPostId);

		Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
	}
}

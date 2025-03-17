using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCBlogWebsite.Models.Domain;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;

namespace MVCBlogWebsite.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _blogPostLikeRepository = blogPostLikeRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();

            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    //Get like for this blog for this user
                    var likesForBlog = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
                    var userId = _userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null; //change liked to true if the userid is not null
                    }
                }

                //get comments for blog post
                var blogCommentsDomainModel = await _blogPostCommentRepository.GetCommentsByBlogIdIAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName //finding user from userManager class by the id, then getting the UserName of that ID
                    });
                }

                //convert domain model into view model
                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await _blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
        }
    }
}

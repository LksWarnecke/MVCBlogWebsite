using Microsoft.AspNetCore.Mvc;
using MVCBlogWebsite.Models;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;
using System.Diagnostics;

namespace MVCBlogWebsite.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
		{
			_logger = logger;
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }

		public async Task<IActionResult> Index()
		{
			//getting all blogs
			var blogPosts = await _blogPostRepository.GetAllAsync();

			//getting all tags
			var tags = await _tagRepository.GetAllAsync();

			var model = new HomeViewModel
			{
				BlogPosts = blogPosts,
				Tags = tags
			};

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

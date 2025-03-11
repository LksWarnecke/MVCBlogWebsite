using Microsoft.AspNetCore.Mvc;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCBlogWebsite.Models.Domain;

namespace MVCBlogWebsite.Controllers
{
	public class AdminBlogPostsController : Controller
	{
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
		public async Task<IActionResult> Add()
		{
			//get tags from repository
			var tags = await _tagRepository.GetAllAsync();

			var model = new AddBlogPostRequest
			{
				Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
		{
			// Map view model to domain model
			var blogPost = new BlogPost
			{
				Heading = addBlogPostRequest.Heading,
				PageTitle = addBlogPostRequest.PageTitle,
				Content = addBlogPostRequest.Content,
				ShortDescription = addBlogPostRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostRequest.UrlHandle,
				PublishedDate = addBlogPostRequest.PublishedDate,
				Author = addBlogPostRequest.Author,
				Visible = addBlogPostRequest.Visible,
			};

			//Map Tags from Selected Tags
			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);

				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}

			//Mapping Tags back to Domain model
			blogPost.Tags = selectedTags;

			await _blogPostRepository.AddAsync(blogPost);

			return RedirectToAction("Add");
		}
	}
}

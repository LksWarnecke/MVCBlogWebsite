﻿using Microsoft.AspNetCore.Mvc;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;

namespace MVCBlogWebsite.Controllers
{
	public class BlogsController : Controller
	{
		private readonly IBlogPostRepository _blogPostRepository;
		private readonly IBlogPostLikeRepository _blogPostLikeRepository;

		public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository)
		{
			_blogPostRepository = blogPostRepository;
			_blogPostLikeRepository = blogPostLikeRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index(string urlHandle)
		{
			var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
			var blogDetailsViewModel = new BlogDetailsViewModel();

			if (blogPost != null)
			{
				var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);
				
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
					TotalLikes = totalLikes
				};
			}

			return View(blogDetailsViewModel);
		}
	}
}

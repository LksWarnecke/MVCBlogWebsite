﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCBlogWebsite.Models.Domain;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;

namespace MVCBlogWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            _blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId
            };

            await _blogPostLikeRepository.AddLikeForBlog(model);

            return Ok();
        }


        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task <IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPostId);

            return Ok(totalLikes);
        }
    }
}

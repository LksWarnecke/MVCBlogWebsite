using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Data;
using MVCBlogWebsite.Models.Domain;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;

namespace MVCBlogWebsite.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminTagsController : Controller
	{
		private readonly ITagRepository _tagRepository;

		public AdminTagsController(ITagRepository tagRepository)
        {
			_tagRepository = tagRepository;
		}

        [HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost] //when submitting form it gets posted to Post Method
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
			ValidateAddTagRequest(addTagRequest);

			if (ModelState.IsValid == false)
			{
				return View();
			}

			//Mapping AddTagRequest to Tag domain model
			var tag = new Tag
			{
				Name = addTagRequest.Name,
				DisplayName = addTagRequest.DisplayName,
			};

			await _tagRepository.AddAsync(tag);

			return RedirectToAction("List");
		}

		[HttpGet]
		[ActionName("List")]
		public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 3, int pageNumber = 1)
		{
			var totalRecords = await _tagRepository.CountAsync();
			var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

			if (pageNumber > totalPages)
			{
				pageNumber--;
			}

			if (pageNumber < 1)
			{
				pageNumber++;
			}

			ViewBag.TotalPages = totalPages;

			ViewBag.SearchQuery = searchQuery;
			ViewBag.SortBy = sortBy;
			ViewBag.SortDirection = sortDirection;
			ViewBag.PageSize = pageSize;
			ViewBag.PageNumber = pageNumber;

            //use dbContext to read the tags
            var tags = await _tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

			return View(tags);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			var tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
				var editTagRequest = new EditTagRequest
				{
					Id = tag.Id,
					Name = tag.Name,
					DisplayName = tag.DisplayName,
				};

				return View(editTagRequest);
            }

            return View(null);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag
			{
				Id = editTagRequest.Id,
				Name = editTagRequest.Name,
				DisplayName = editTagRequest.DisplayName,
			};
			
			var updatedTag = await _tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success notification
            }
			else
			{
				//show error notification
			}

            return RedirectToAction("Edit", new { id = editTagRequest.Id });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

			if (deletedTag != null)
			{
				//show success noti.
				return RedirectToAction("List");
			}

			//show error notif.
			return RedirectToAction("Edit", new { id = editTagRequest.Id });
		}

		private void ValidateAddTagRequest(AddTagRequest addTagRequest)
		{
			if (addTagRequest?.Name is not null && addTagRequest?.DisplayName is not null)
			{
				if (addTagRequest.Name == addTagRequest.DisplayName)
				{
					ModelState.AddModelError("DisplayName", "Name cannot be the same as DisplayName");
				}
			}
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBlogWebsite.Data;
using MVCBlogWebsite.Models.Domain;
using MVCBlogWebsite.Models.ViewModels;
using MVCBlogWebsite.Repositories;

namespace MVCBlogWebsite.Controllers
{
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
		public async Task<IActionResult> List()
		{
			//use dbContext to read the tags
			var tags = await _tagRepository.GetAllAsync();

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

	}
}

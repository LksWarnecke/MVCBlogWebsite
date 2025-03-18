using Microsoft.AspNetCore.Identity;

namespace MVCBlogWebsite.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}

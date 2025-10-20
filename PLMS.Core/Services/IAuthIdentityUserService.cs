using PLMS.Core.DTOs;

namespace PLMS.Core.Services
{
    public interface IAuthIdentityUserService
    {
        Task<List<AuthIdentityUserForAdminDto>> GetAllUsersAsync();
    }
}

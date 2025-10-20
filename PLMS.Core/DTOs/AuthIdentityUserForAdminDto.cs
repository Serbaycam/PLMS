using PLMS.Core.Entities;

namespace PLMS.Core.DTOs
{
    public class AuthIdentityUserForAdminDto:AuthIdentityUser
    {
        public List<string> Roles { get; set; }
    }
}

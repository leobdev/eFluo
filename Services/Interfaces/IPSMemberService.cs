using eFluo.Models;

namespace eFluo.Services.Interfaces
{
    public interface IPSMemberService
    {
        public Task<PSUser> GetMemberByIdAsync(int companyId, string userId);
        public Task<bool> RemoveMemberAsync(PSUser member);
    }
}

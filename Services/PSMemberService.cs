using Microsoft.EntityFrameworkCore;
using eFluo.Data;
using eFluo.Models;
using eFluo.Services.Interfaces;

namespace eFluo.Services
{
    public class PSMemberService : IPSMemberService
    {
        private readonly ApplicationDbContext _context;

        public PSMemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PSUser> GetMemberByIdAsync(int companyId, string userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(user => user.CompanyId == companyId)
                    .FirstOrDefaultAsync(user => user.Id == userId);

                return user ?? throw new Exception("User not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveMemberAsync(PSUser member)
        {
            try
            {
                _context.Users.Remove(member);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}

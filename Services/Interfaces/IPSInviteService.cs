﻿using eFluo.Models;

namespace eFluo.Services.Interfaces
{
    public interface IPSInviteService
    {

        public Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId);

        public Task AddNewinviteAsync(Invite invite);

        public Task<bool> AnyInviteAsync(Guid token, string email, int companyId);

        public Task<Invite> GetInviteAsync(int inviteId, int companyId);

        public Task<Invite> GetInviteAsync(Guid token, string email, int companyId);

        public Task<bool> ValidateInviteCodeAsync(Guid? token);
    }
}

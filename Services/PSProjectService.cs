﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eFluo.Data;
using eFluo.Models;
using eFluo.Models.Enums;
using eFluo.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace eFluo.Services
{
    public class PSProjectService : IPSProjectService
    {

        private readonly ApplicationDbContext _context;
        private readonly IPSRolesService _rolesService;
        private readonly UserManager<PSUser> _userManager;

        public PSProjectService(ApplicationDbContext context, IPSRolesService rolesService, UserManager<PSUser> userManager)
        {
            _context = context;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            PSUser currentPM = await GetProjectManagerAsync(projectId);

            if (currentPM != null)
            {
                try
                {
                    await RemoveProjectManagerAsync(projectId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"***ERROR*** Error removing PM. Error --> {ex.Message}");

                    return false;
                }
            }

            //Add the new PM

            try
            {
                await AddUserToProjectAsync(userId, projectId);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"***ERROR*** Error adding PM. Error --> {ex.Message}");

                return false;
            }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            PSUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                if (!await IsUserOnProjectAsync(userId, projectId))
                {
                    try
                    {
                        project.Members.Add(user);

                        await _context.SaveChangesAsync();

                        return true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public async Task ArchiveProjectAsync(Project project)
        {
            try
            {
                project.Archived = true;
                await UpdateProjectAsync(project);

                //Archive the Tickets for the Project
                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = true;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<PSUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            List<PSUser> developers = await GetProjectMembersByRoleAsync(projectId, Roles.Developer.ToString());
            List<PSUser> submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Submitter.ToString());
            List<PSUser> admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());

            List<PSUser> teamMembers = developers.Concat(submitters).Concat(admins).ToList();

            return teamMembers;
        }

        public async Task<List<Project>> GetAllProjectsByCompanyAsync(int companyId)
        {
            List<Project> projects = new();

            try
            {

                projects = await _context.Projects.Where(x => x.CompanyId == companyId && x.Archived == false)
                                                .Include(x => x.Members)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.Comments)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.TicketStatus)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.TicketPriority)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.TicketType)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.Histories)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.DeveloperUser)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.OwnerUser)
                                                .Include(x => x.Tickets)
                                                    .ThenInclude(t => t.Notifications)
                                                .Include(x => x.ProjectPriority)
                                                .ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }


            return projects;
        }

        public async Task<List<Project>> GetAllProjectsByPriorityAsync(int companyId, string priorityName)
        {
            List<Project> projects = await GetAllProjectsByCompanyAsync(companyId);

            int priorityId = await LookupProjectPriorityId(priorityName);

            return projects.Where(x => x.ProjectPriorityId == priorityId).ToList();
        }

        public async Task<List<Project>> GetArchivedProjectsByCompanyAsync(int companyId)
        {
            try
            {
                List<Project> projects = await _context.Projects.Where(x => x.CompanyId == companyId && x.Archived == true)
                                .Include(x => x.Members)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.Comments)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.TicketStatus)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.TicketPriority)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.TicketType)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.Histories)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.DeveloperUser)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.OwnerUser)
                                .Include(x => x.Tickets)
                                    .ThenInclude(t => t.Notifications)
                                .Include(x => x.ProjectPriority)
                                .ToListAsync();

                return projects;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<List<PSUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            /*            Project project = await _context.Projects
                .Include(x => x.Tickets)
                .Include(x => x.Members)
                .Include(x => x.ProjectPriority)
                .FirstOrDefaultAsync(x => x.Id == projectId && x.CompanyId == companyId);
*/
            try
            {
                Project project = await _context.Projects
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketPriority)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketStatus)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.TicketType)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.DeveloperUser)
                                        .Include(p => p.Tickets)
                                            .ThenInclude(t => t.OwnerUser)
                                        .Include(p => p.Members)
                                        .Include(p => p.ProjectPriority)
                                        .FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);


                return project;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<PSUser> GetProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                                .Include(x => x.Members)
                                .FirstOrDefaultAsync(p => p.Id == projectId);

            foreach (PSUser member in project?.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                {
                    return member;
                }
            }
            return null;
        }

        public async Task<List<PSUser>> GetProjectMembersByRoleAsync(int id, string role)
        {
            Project project = await _context.Projects.Include(x => x.Members)
                                     .FirstOrDefaultAsync(t => t.Id == id);

            List<PSUser> members = new();


            foreach (PSUser user in project.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(user, role))
                {
                    members.Add(user);
                }
            }
            return members;
        }

        public Task<List<PSUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUnassignedProjectsAsync(int companyId)
        {
            List<Project> result = new();
            List<Project> projects = new();

            try
            {
                projects = await _context.Projects
                                .Include(p => p.ProjectPriority)
                                .Where(p => p.CompanyId == companyId).ToListAsync();

                foreach (Project project in projects)
                {
                    if ((await GetProjectMembersByRoleAsync(project.Id, nameof(Roles.ProjectManager))).Count() == 0)
                    {
                        result.Add(project);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project> userProjects = (await _context.Users
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Company)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Members)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                                    .ThenInclude(u => u.DeveloperUser)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                                    .ThenInclude(u => u.OwnerUser)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                                    .ThenInclude(u => u.TicketPriority)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                                    .ThenInclude(u => u.TicketStatus)
                                            .Include(x => x.Projects)
                                                .ThenInclude(t => t.Tickets)
                                                    .ThenInclude(u => u.TicketType)
                                            .FirstOrDefaultAsync(x => x.Id == userId)).Projects.ToList();

                return userProjects;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"****ERROR***** - Error getting user projects list ---> {ex.Message}");

                throw;

            }
        }

        public async Task<List<PSUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            List<PSUser> users = await _context.Users.Where(x => x.Projects.All(p => p.Id != projectId)).ToListAsync();

            return users.Where(x => x.CompanyId == companyId).ToList();
        }

        public async Task<bool> IsAssignedProjectManagerAsync(string userId, int projectId)
        {
            try
            {
                string projectManagerId = (await GetProjectManagerAsync(projectId))?.Id;

                if (projectManagerId == userId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsUserOnProjectAsync(string userId, int projectId)
        {
            Project project = await _context.Projects
                               .Include(x => x.Members)
                               .FirstOrDefaultAsync(x => x.Id == projectId);

            bool result = false;

            if (project != null)
            {
                result = project.Members.Any(x => x.Id == userId);
            }

            return result;
        }

        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            int priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(x => x.Name == priorityName)).Id;

            return priorityId;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                                .Include(x => x.Members)
                                .FirstOrDefaultAsync(p => p.Id == projectId);
            try
            {
                foreach (PSUser member in project?.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        await RemoveUserFromProjectAsync(member.Id, projectId);

                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                PSUser user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                Project project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);

                        await _context.SaveChangesAsync();
                    }

                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"*****Error****** Error removing user from project. ---> {ex.Message}");
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                List<PSUser> members = await GetProjectMembersByRoleAsync(projectId, role);

                Project project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

                foreach (PSUser btUser in members)
                {
                    try
                    {
                        project.Members.Remove(btUser);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"****ERROR**** - Error removing users from project. ---> {ex.Message}");

                throw;
            }
        }

        public async Task RestoreProjectAsync(Project project)
        {
            try
            {
                project.Archived = false;
                await UpdateProjectAsync(project);

                //Archive the Tickets for the Project
                foreach (Ticket ticket in project.Tickets)
                {
                    ticket.ArchivedByProject = false;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

      
    }
}

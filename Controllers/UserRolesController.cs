﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using eFluo.Extensions;
using eFluo.Models;
using eFluo.Models.Enums;
using eFluo.Models.ViewModels;
using eFluo.Services.Interfaces;
using System.Collections;
using System.Diagnostics.Eventing.Reader;

namespace ProbSolv.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly IPSRolesService _rolesService;
        private readonly IPSCompanyInfoService _companyInfoService;
        private readonly UserManager<PSUser> _userManager;
        public UserRolesController(IPSRolesService rolesService, IPSCompanyInfoService companyInfoService, UserManager<PSUser> userManager)
        {
            _rolesService = rolesService;
            _companyInfoService = companyInfoService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new();
             
            int companyId = User.Identity.GetCompanyId().Value;
            List<PSUser> users = await _companyInfoService.GetAllMembersAsync(companyId);

            var roles = await _rolesService.GetRolesAsync();
          

           

            foreach (var user in users)
            {

                var selected = await _rolesService.GetUserRolesAsync(user);

                ManageUserRolesViewModel vm = new ManageUserRolesViewModel()
                {
                    PSUser = user,
                    Roles = new SelectList(roles, "Name", "Name", selected.FirstOrDefault()),
                    SelectedRole = selected.FirstOrDefault(),
                    UserRoles = await _userManager.GetRolesAsync(user)
                };            

                model.Add(vm);
            }

            return View(model);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel item, string userId)
        {
            // Get the companyId
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate the PSUser
            PSUser user = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == userId);

            //Get Roles for the user
            //IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(user);

            

            //Add user to the new role
            await _rolesService.AddUserRoleAsync(user, item.SelectedRole);
             
            //Navigate back to the view
            return RedirectToAction(nameof(ManageUserRoles));

        }


        [HttpGet]
        public async Task<IActionResult> RemoveRole(string role, string userId)
        {
            // Get the companyId
            int companyId = User.Identity.GetCompanyId().Value;

            //Instantiate the PSUser
            PSUser user = (await _companyInfoService.GetAllMembersAsync(companyId)).FirstOrDefault(u => u.Id == userId);

            await _rolesService.RemoveUserFromRoleAsync(user, role);

            return RedirectToAction(nameof(ManageUserRoles));
        }




    }
}

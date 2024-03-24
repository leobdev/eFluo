using Microsoft.AspNetCore.Mvc.Rendering;

namespace eFluo.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public PSUser PSUser { get; set; }

        public SelectList Roles { get; set; }

        public string SelectedRole { get; set; }

        public IList<string> UserRoles { get; set; }

       
    }
}

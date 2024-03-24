using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace eFluo.Models
{
    public class PSUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        [NotMapped]
        [Display(Name = "Name Initials")]
        public string NameInitials { get { return $"{FirstName[0]} {LastName[0]}"; } }  

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? AvatarFormFile { get; set; }

        [DisplayName("Avatar")]
        public string AvatarFileName { get; set; } = string.Empty;

        public byte[]? AvatarFileData { get; set; } 

        [Display(Name = "File Extension")]
        public string AvatarContentType { get; set; } = string.Empty;

        public int CompanyId { get; set; }
               

        //Navigation properties

        public virtual Company Company { get; set; } = new Company();

        public virtual ICollection<Project>? Projects { get; set; } = new HashSet<Project>();

    }
}

using System.ComponentModel;

namespace eFluo.Models
{
    public class Company
    {
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Company Description")]
        public string Description { get; set; } = string.Empty;



        //navigation properties

        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        public virtual ICollection<PSUser> Members { get; set; } = new HashSet<PSUser>();

        public virtual ICollection<Invite> Invites { get; set; } = new HashSet<Invite>();
    }
}

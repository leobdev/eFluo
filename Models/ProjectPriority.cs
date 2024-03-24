using System.ComponentModel;

namespace eFluo.Models
{
    public class ProjectPriority
    {
        public int Id { get; set; }

        [DisplayName("Project Name")]
        public string Name { get; set; } = string.Empty;

    }
}

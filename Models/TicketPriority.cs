using System.ComponentModel;

namespace eFluo.Models
{
    public class TicketPriority
    {

        public int Id { get; set; } 

        [DisplayName("Priority")]
        public string Name { get; set; } = string.Empty;
    }
}

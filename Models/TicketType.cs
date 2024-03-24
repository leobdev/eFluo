using System.ComponentModel;

namespace eFluo.Models
{
    public class TicketType
    {
        public int Id { get; set; }


        [DisplayName("Type")]
        public string Name { get; set; } = string.Empty;


    }
}

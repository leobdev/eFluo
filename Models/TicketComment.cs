using System.ComponentModel;

namespace eFluo.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [DisplayName("Member Comment")]
        public string Comment { get; set; } = string.Empty;

        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; } = string.Empty;

        public virtual Ticket Ticket { get; set; }

        public virtual PSUser User { get; set; }


    }
}

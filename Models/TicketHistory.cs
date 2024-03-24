using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eFluo.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Updated Item")]
        public string Property { get; set; } = string.Empty;

        [DisplayName("Previous")]
        public string OldValue { get; set; } = string.Empty;

        [DisplayName("Current")]
        public string NewValue { get; set; } = string.Empty;

        [DisplayName("Date Modified")]
        [DataType(DataType.Date)]
        public DateTimeOffset? Created { get; set; }

        [DisplayName("Description of Change")]
        public string Description { get; set; } = string.Empty;

        [DisplayName("Team Member")]
        public string UserId { get; set; }


        //Navigation Properties
        public virtual Ticket Ticket { get; set; }

        public virtual PSUser User { get; set; }


    }
}

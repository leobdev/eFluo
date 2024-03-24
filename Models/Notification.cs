﻿using System.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eFluo.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int? TicketId { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        [Required]
        [DisplayName("Recipient")]
        public required string  RecipientId { get; set; } 

        [Required]
        [DisplayName("Sender")]
        public string SenderId { get; set; }

        [DisplayName("Has been viewed")]
        public bool Viewed { get; set; }


        //Navigation properties
        public virtual Ticket? Ticket { get; set; }

        public virtual PSUser? Recipient { get; set; }

        public virtual PSUser? Sender { get; set; }


    }
}

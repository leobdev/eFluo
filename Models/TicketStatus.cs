﻿using System.ComponentModel;

namespace eFluo.Models
{
    public class TicketStatus
    {

        public int Id { get; set; } 

        [DisplayName("Status")]
        public string Name { get; set; } = string.Empty;
    }
}

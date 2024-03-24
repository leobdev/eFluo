﻿namespace eFluo.Models.ViewModels
{
    public class DashboardViewModel
    {
        public Company Company { get; set; }

        public List<Project> Projects { get; set; }

        public List<Ticket> Tickets { get; set; }

        public List<PSUser> Members { get; set; }

        public List<Notification> Notifications { get; set; }

    }
}

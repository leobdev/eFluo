﻿using eFluo.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eFluo.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DisplayName("Company")]
        public int? CompanyId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTimeOffset StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTimeOffset EndDate { get; set; }

        [DisplayName("Priority")]
        public int? ProjectPriorityId { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFormFile { get; set; }

        [DisplayName("File Name")]
        public string ImageFileName { get; set; } = string.Empty;

        [DisplayName("Image")]
        public byte[]? ImageFileData { get; set; }

        [DisplayName("File Extension")]
        public string ImageContentType { get; set; } = string.Empty;

        [DisplayName("Archived")]
        public bool Archived { get; set; }


        //Navigation properties

        public virtual Company Company { get; set; }

        public virtual ProjectPriority ProjectPriority { get; set; }

        public virtual ICollection<PSUser> Members { get; set; } = new HashSet<PSUser>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

    }
}

using eFluo.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eFluo.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; }

        [DisplayName("Team Member")]
        public string UserId { get; set; } = string.Empty;

        [DisplayName("File Description")]
        public string FileDescription { get; set; } = string.Empty;

        [NotMapped]
        [DisplayName("Select a file")]
        [DataType(DataType.Upload)]
        [MaxFileSize(1024 * 1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".doc", ".docx", ".xls", ".xlsx", ".pdf" })] 
        public IFormFile FormFile { get; set; }

        [DisplayName("File Name")]
        public string FileName { get; set; } = string.Empty;

        public byte[]? FileData { get; set; } 

        [DisplayName("File Extension")]
        public string FileContentType { get; set; } = string.Empty;


        //Navigation properties
        public virtual Ticket Ticket { get; set; }

        public virtual PSUser User { get; set; }

    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContractMonitoringSystem.Data
{
    public class FileUpload
    {
        [Key]
        public int FileId { get; set; }
        [Required]
        public string FileName { get; set; }
        [NotMapped]
        public string FilePath { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] FileContent { get; set; }
        public int? UserId { get; set; }
        public long Size { get; set; }
        [Column("CMSiteId")]
        public int? CMSiteId { get; set; }
        [ForeignKey("CMSiteId")]
        [NotMapped]
        public SiteInstruction? SiteInstruction { get; set; }
        [Column("PotId")]
        public int? PotId { get; set; }
        [ForeignKey("PotId")]
        [NotMapped]
        public PotentialClaim? PotentialClaim { get; set; }

        //[ForeignKey("EOTId")]
        //public int? EOTId { get; set; }
        //[ForeignKey("EOTId")]
        //[NotMapped]
        //public EOTClaim? EOTClaim { get; set; }
        //[Column("BillingId")]
        //[ForeignKey("BillingId")]
        //public int? BillingId { get; set; }

        ////   [NotMapped]
        //public Billing? Billings { get; set; }
    }
}

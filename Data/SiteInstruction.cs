using ContractMonitoringSystem.Models.MainProperties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Data
{
    public class SiteInstruction: MainAmountVM
    {
        [Key]
        public int CMSiteId { get; set; }
        [Required]
        [Display(Name = "CM-PMI Number")]
        public string? CMPMINumber { get; set; }
        [Required]
        [Display(Name = "Asec-PMI Number")]
        public string? AsecPMINumber { get; set; }
        [NotMapped]
        public string? EngRemarks { get; set; }
        public decimal? EquivAmount { get; set; }
    }
}

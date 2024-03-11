using ContractMonitoringSystem.Models.MainProperties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Data
{
    public class PotentialClaim: MainAmountVM
    {
        [Key]
        public int PotId { get; set; }
        [Required]
        [Display(Name = "CVI Number")]
        public string CVINumber { get; set; }
        [Required]
        [Display(Name = "Asec-PMI Number")]
        public string AsecPMINumber { get; set; }
        [NotMapped]
        public string? EngRemarks { get; set; }
        public int? EquivAmount { get; set; }

    }
}

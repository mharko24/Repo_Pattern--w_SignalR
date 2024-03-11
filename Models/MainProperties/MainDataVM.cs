using ContractMonitoringSystem.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Models.MainProperties
{
    public class MainDataVM: MainListDataVM
    {

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required]
        public string Remarks { get; set; }
        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Open";
        //[Required]
        //[Display(Name = "PO Number")]
        public string? PONumber { get; set; }
        [NotMapped]
        public string? ProjectCode { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ContractMonitoringSystem.Data
{
    public class ProjectDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProjectCode { get; set; }
        [Required]
        public string ProjectName { get; set; }
        public int? OrigContractAmount { get; set; }
        [Required]
        public string Address { get; set; }
        public string? PONumber { get; set; }
        public decimal? ProjectCompletion { get; set; }
        public int Active { get; set; }
    }
}

using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Models.MainProperties;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Models.ViewModel
{
    public class MainListDataVM: MainFileUploadVM
    {

        //public IList<SiteInstruction>? SiteList { get; set; }
        [NotMapped]
        public IEnumerable<PotentialClaim>? PotList { get; set; }
        // [NotMapped]
        // public List<string>? ProjectList { get; set; }
        [NotMapped]
        public List<ProjectCodeAndNameVM>? ProjectList { get; set; }
        [NotMapped]
        public List<FileUpload>? FileUpload { get; set; }
        [NotMapped]
        public List<FileUpload>? FileUpload2 { get; set; }
        [NotMapped]
        public List<AmountHistory>? AmountList { get; set; } 


    }
}

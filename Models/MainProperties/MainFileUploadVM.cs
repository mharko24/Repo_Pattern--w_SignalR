using ContractMonitoringSystem.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Models.MainProperties
{
    public class MainFileUploadVM
    {
        [Column("UserId")]
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public UserApp? UserApps { get; set; }
        [NotMapped]
        public IList<IFormFile> Files { get; set; }
    }
}

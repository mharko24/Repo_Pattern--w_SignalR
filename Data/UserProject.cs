using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContractMonitoringSystem.Data
{
    public class UserProject
    {
        [Key]
        [Column("UPId")]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int? UserId { get; set; }
        [NotMapped]
        public UserApp? UserApp { get; set; }
        [ForeignKey("ProdId")]
        public int? ProdId { get; set; }
    }

}

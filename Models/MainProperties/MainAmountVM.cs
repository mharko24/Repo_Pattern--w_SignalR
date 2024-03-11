using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonitoringSystem.Models.MainProperties
{
    public class MainAmountVM: MainDataVM
    {
        public int? Amount { get; set; }
        public int? Completion { get; set; }
        
        [NotMapped]
        public bool boolAdditive { get; set; }
        [NotMapped]
        public bool boolDeductive { get; set; }
        public int? Additive { get; set; }
        public int? Deductive { get; set; }

        public int? BilledCompletion { get; set; }
        public int? EquivBilled { get; set; }
    }
}

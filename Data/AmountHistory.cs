namespace ContractMonitoringSystem.Data
{
    public class AmountHistory
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int Amount { get; set; }
        public int? CMSiteId { get; set; }
        public int? PotId { get; set; }
        public DateTime date { get; set; }
    }
}

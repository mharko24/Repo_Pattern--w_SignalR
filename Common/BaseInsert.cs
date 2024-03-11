using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Models;

namespace ContractMonitoringSystem.Common
{
    public class BaseInsert
    {
        private readonly ApplicationDbContext _db;
        private string Site = "SiteInstruction";
        private string Pot = "PotentialClaim";
        private string EOT = "EOTClaim";
        private string bill = "Billing";
        public BaseInsert(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public void InsertAmount(string type, int id, int? Amount, int userId)
        {
            if (type != null)
            {
                var model = new AmountHistory()
                {
                    Amount = Amount ?? 0,
                    date = DateTime.Now,
                    userId = userId,
                    CMSiteId = type == Site ? id : null,
                    PotId = type == Pot ? id : null,
                };
                _db.AmountHistories.Add(model);
                _db.SaveChanges();


            }
        }
    }
}

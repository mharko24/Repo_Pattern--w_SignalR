using Microsoft.AspNetCore.Mvc;

namespace ContractMonitoringSystem.Controllers
{
    public class BaseController:Controller
    {
        public int userId
        {
            get
            {
                var userClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
                return int.TryParse(userClaim?.Value, out int userId) ? userId : 0;
            }
        }
    }
}

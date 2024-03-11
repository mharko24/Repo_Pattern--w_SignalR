using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractMonitoringSystem.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserApp> GetUserDetails(string username, string password)
        {
            var user = await _dbContext.UserApps.FirstOrDefaultAsync(user => user.Username == username && user.Password == password);
            if (user != null) return user;
            else return default;



        }
        
    }
}

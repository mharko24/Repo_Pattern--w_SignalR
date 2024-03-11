using ContractMonitoringSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace ContractMonitoringSystem.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<SiteInstruction>SiteInstructions {get;set;}
        public virtual DbSet<FileUpload>FileUploads{get;set;}
        public virtual DbSet<UserApp>UserApps{get;set;}
        public virtual DbSet<ProjectDetail> ProjectDetails{ get;set;}
        public virtual DbSet<UserProject> UserProjects { get;set;}
        public virtual DbSet<AmountHistory> AmountHistories{ get;set;}
        public virtual DbSet<PotentialClaim> PotentialClaims{ get;set;}
    }
}

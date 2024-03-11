using ContractMonitoringSystem.Interfaces;
using ContractMonitoringSystem.Models;
using ContractMonitoringSystem.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ContractMonitoringSystem.Repositories
{
    public class GetProjectRepository : IGetProjectRepository
    {
        private readonly ApplicationDbContext _db;
        public GetProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public List<ProjectCodeAndNameVM> EngProjectNameAndCode(int id)
        {
            var projectlist = _db.UserProjects
                .Where(x => x.UserId == id)
                .Join(_db.ProjectDetails,
                x => x.ProdId,
                y => y.Id,
                (x, y) => new ProjectCodeAndNameVM
                {
                    ProjectCode = y.ProjectCode,
                    ProjectName = y.ProjectName
                }).ToList();
            return projectlist;
        }

        public List<ProjectCodeAndNameVM> AdminProjectNameAndCode()
        {
            var projectList = _db.ProjectDetails.Select(x => new ProjectCodeAndNameVM { ProjectCode = x.ProjectCode, ProjectName = x.ProjectName }).ToList();
            return projectList;
        }

        public ProjectCodeAndNameVM CostProjectCodeAndName(int id)
        {
            var CostProject = _db.UserProjects
                .Where(u => u.UserId == id)
                .Select(u => u.ProdId)
            .Join(
                    _db.ProjectDetails,
                    u => u,
                    lp => lp.Id,
                    (u, lp) => new ProjectCodeAndNameVM
                    {
                        ProjectCode = lp.ProjectCode,
                        ProjectName = lp.ProjectName
                    }
                )
                .FirstOrDefault();
            return CostProject;
        }
    }
}

using Azure.Core;
using ContractMonitoringSystem.Common;
using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Hubs;
using ContractMonitoringSystem.Interfaces;
using ContractMonitoringSystem.Models;
using ContractMonitoringSystem.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq;

namespace ContractMonitoringSystem.Controllers
{
    public class SiteInstructionController : BaseController
    {
        private readonly IBaseRepository<SiteInstruction> _baseRepo;
        private readonly IBaseRepository<FileUpload> _fileRepo;
        private readonly IBaseRepository<AmountHistory> _amountRepo;
        private readonly ApplicationDbContext _db;
        private readonly IBaseUpload<SiteInstruction> _baseUpload;
        private readonly BaseInsert _baseInsert;
        private readonly IGetProjectRepository _getProject;
        private readonly IHubContext<SiteHub> _siteHub;
        public SiteInstructionController(IBaseRepository<SiteInstruction> baseRepo,ApplicationDbContext db, IBaseRepository<FileUpload> fileRepo, IBaseUpload<SiteInstruction> baseUpload, IBaseRepository<AmountHistory> amountRepo, BaseInsert baseInsert,
            IGetProjectRepository getProject, IHubContext<SiteHub> siteHub)
        {
            _baseRepo = baseRepo;
            _db = db;
            _fileRepo = fileRepo;
            _baseUpload = baseUpload;
            _amountRepo = amountRepo;
            _baseInsert = baseInsert;
            _getProject = getProject;
            _siteHub = siteHub;
        }
        public async Task<IActionResult> Index(int pageCurrent, int numSize,string SearhKeyword)
        {
            if(pageCurrent!=0 && numSize!=0)
            {
               var results=  await _baseRepo.GetPaginated(pageCurrent, numSize, x=>x.Date, x => x.ProjectName.Contains(SearhKeyword ?? string.Empty));
                return Json(results);
            }
            
            return View();
            
            
        }
        public async Task<IActionResult> GetData(PaginatedRequest request)
        {
            if (User.IsInRole("Cost"))
            {
                var CostProject = _getProject.CostProjectCodeAndName(userId);
                var siteList = await _baseRepo.GetPaginated(request.PageNumber, request.ItemsPerPage, x => x.Date, x => x.ProjectName == CostProject.ProjectName && x.UserId == userId);
                return Json(siteList);
            }
            else if (User.IsInRole("Engineering"))
            {
                var model = new PaginatedResult<SiteInstruction>();
                var EngProject = _getProject.EngProjectNameAndCode(userId);
                var getProjectNames = EngProject.Select(x => x.ProjectName).ToList();

                var allSiteInstructions = await _baseRepo.GetPaginated(
                    request.PageNumber,
                    request.ItemsPerPage,
                    x => x.Date,
                    x => true // Retrieve all records without applying filtering yet
                );

                // Filter the fetched site instructions in memory
                var siteList = allSiteInstructions.data.Where(x =>
                    getProjectNames.Any(projectName => x.ProjectName.Contains(projectName))
                ).ToList();

                model.data = siteList;
                return Json(model);
            }
            else
            {
                var siteList = await _baseRepo.GetPaginated(request.PageNumber, request.ItemsPerPage, x => x.Date, x => x.ProjectName.Contains(request.SearhKeyword ?? string.Empty));
                return Json(siteList);
            }



        }
        public  IActionResult Create ()
        {
            var model = new SiteInstruction();
            if (User.IsInRole("Cost"))
            {
                var CostProject = _getProject.CostProjectCodeAndName(userId);
                model.ProjectName = CostProject.ProjectName;
                return View(model);
            }
            else
            {
                model.ProjectList = _getProject.AdminProjectNameAndCode();
                return View(model);
            }

            
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Cost")]
        [HttpPost]
        public async Task<IActionResult> Create(SiteInstruction site)
        {
            if (site != null)
            {
                site.ProjectList = _getProject.AdminProjectNameAndCode();
                if (string.IsNullOrEmpty(site.ProjectName) || string.IsNullOrEmpty(site.CMPMINumber) || string.IsNullOrEmpty(site.AsecPMINumber)|| string.IsNullOrEmpty(site.Remarks))
                {
                    ViewBag.Error = "Please verify your entry";
                    return View(site);
                }
                else if ((!site.boolAdditive && !site.boolDeductive) || (site.boolAdditive && site.boolDeductive))
                {
                    ViewBag.Error = "Please select one from the checkboxes!";
                    return View(site);
                }
                else
                {
                    site.Additive = site.boolAdditive ? 1 : 0;
                    site.Deductive = site.boolDeductive ? 1 : 0;
                    site.Status = "For Costing";
                    site.UserId = userId;
                    await _baseRepo.Create(site);
                    
                    if (site.Files != null) await _baseUpload.UploadFile(site, site.Files, site.UserId, site.CMSiteId);
                    await _siteHub.Clients.All.SendAsync("LoadSiteData");
                    return RedirectToAction("Index");
                }
            }
            return View(site);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var site = await _baseRepo.GetOne(id);
            if(site!=null)
            {
                site.FileUpload = _fileRepo.Where(x => x.CMSiteId == id).ToList();
                site.AmountList = _amountRepo.Where(x => x.CMSiteId == id).ToList();
                return View(site);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var site = await _baseRepo.GetOne(id);
            if (site != null)
            {
                site.FileUpload = _fileRepo.Where(x => x.CMSiteId == id).ToList();
                site.AmountList = _amountRepo.Where(x => x.CMSiteId == id).ToList();
                return View(site);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SiteInstruction site)
        {
            if(site!=null)
            {
                site.FileUpload = _fileRepo.Where(x => x.CMSiteId == site.CMSiteId).ToList();
                if ((!site.boolAdditive && !site.boolDeductive) || (site.boolAdditive && site.boolDeductive))
                {
                    ViewBag.Error = "Please select one from the checkboxes!";
                    return View(site);
                }
                else
                {
                    site.Additive = site.boolAdditive ? 1 : 0;
                    site.Deductive = site.boolDeductive ? 1 : 0;
                    if (site.Files != null) await _baseUpload.UploadFile(site, site.Files, site.UserId, site.CMSiteId);
                    _baseInsert.InsertAmount("SiteInstruction", site.CMSiteId, site.Amount, userId);
                    await _baseRepo.Update(site.CMSiteId, site);
                    await _siteHub.Clients.All.SendAsync("LoadSiteData");
                    return RedirectToAction("Index");
                }

            }
            return View(site);
        }
        //public IActionResult GetData()

    }
}

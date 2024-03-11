using ContractMonitoringSystem.Common;
using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Interfaces;
using ContractMonitoringSystem.Models.ViewModel;
using ContractMonitoringSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContractMonitoringSystem.Controllers
{
    public class PotentialClaimController : BaseController
    {
        private readonly IBaseRepository<PotentialClaim> _baseRepo;
        private readonly IBaseRepository<FileUpload> _fileRepo;
        private readonly IBaseRepository<AmountHistory> _amountRepo;
        private readonly ApplicationDbContext _db;
        private readonly IBaseUpload<PotentialClaim> _baseUpload;
        private readonly BaseInsert _baseInsert;
        public PotentialClaimController(IBaseRepository<PotentialClaim> baseRepo, ApplicationDbContext db, IBaseRepository<FileUpload> fileRepo, IBaseUpload<PotentialClaim> baseUpload, IBaseRepository<AmountHistory> amountRepo, BaseInsert baseInsert)
        {
            _baseRepo = baseRepo;
            _db = db;
            _fileRepo = fileRepo;
            _baseUpload = baseUpload;
            _amountRepo = amountRepo;
            _baseInsert = baseInsert;
        }
        public async Task<IActionResult> Index(PaginatedRequest request)
        {
            var potList = await _baseRepo.GetPaginated(request.PageNumber, request.ItemsPerPage, x => x.Date, x => x.ProjectName.Contains(request.SearhKeyword ?? string.Empty) || x.Status.Contains(request.SearhKeyword ?? string.Empty));
            return View(potList);
        }
        public IActionResult Create()
        {
            var model = new PotentialClaim()
            {
                ProjectList = _db.ProjectDetails.Select(x => new ProjectCodeAndNameVM { ProjectCode = x.ProjectCode, ProjectName = x.ProjectName }).ToList(),
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PotentialClaim pot)
        {
            if (pot != null)
            {
                pot.ProjectList = _db.ProjectDetails.Select(x => new ProjectCodeAndNameVM { ProjectCode = x.ProjectCode, ProjectName = x.ProjectName }).ToList();
                if (string.IsNullOrEmpty(pot.ProjectName) || string.IsNullOrEmpty(pot.CVINumber) || string.IsNullOrEmpty(pot.AsecPMINumber) || string.IsNullOrEmpty(pot.Remarks))
                {
                    ViewBag.Error = "Please verify your entry";
                    return View(pot);
                }
                else if ((!pot.boolAdditive && !pot.boolDeductive) || (pot.boolAdditive && pot.boolDeductive))
                {
                    ViewBag.Error = "Please select one from the checkboxes!";
                    return View(pot);
                }
                else
                {
                    pot.Additive = pot.boolAdditive ? 1 : 0;
                    pot.Deductive = pot.boolDeductive ? 1 : 0;
                    pot.Status = "For Costing";
                    pot.UserId = userId;
                    await _baseRepo.Create(pot);
                   
                    if (pot.Files != null) await _baseUpload.UploadFile(pot, pot.Files, pot.UserId, pot.PotId);
                    return RedirectToAction("Index");
                }
            }
            return View(pot);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var pot = await _baseRepo.GetOne(id);
            if (pot != null)
            {
                pot.FileUpload = _fileRepo.Where(x => x.PotId == id).ToList();
                pot.AmountList = _amountRepo.Where(x => x.PotId == id).ToList();
                return View(pot);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pot = await _baseRepo.GetOne(id);
            if (pot != null)
            {
                pot.FileUpload = _fileRepo.Where(x => x.PotId == id).ToList();
                pot.AmountList = _amountRepo.Where(x => x.PotId == id).ToList();
                return View(pot);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(PotentialClaim pot)
        {
            if (pot != null)
            {
                pot.FileUpload = _fileRepo.Where(x => x.CMSiteId == pot.PotId).ToList();
                if ((!pot.boolAdditive && !pot.boolDeductive) || (pot.boolAdditive && pot.boolDeductive))
                {
                    ViewBag.Error = "Please select one from the checkboxes!";
                    return View(pot);
                }
                else
                {
                    pot.Additive = pot.boolAdditive ? 1 : 0;
                    pot.Deductive = pot.boolDeductive ? 1 : 0;
                    if (pot.Files != null) await _baseUpload.UploadFile(pot, pot.Files, pot.UserId, pot.PotId);
                    _baseInsert.InsertAmount("PotentialClaim", pot.PotId, pot.Amount,userId);
                    await _baseRepo.Update(pot.PotId, pot);
                    return RedirectToAction("Index");
                }

            }
            return View(pot);
        }

    }
}

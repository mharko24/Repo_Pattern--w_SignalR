using ContractMonitoringSystem.Data;
using ContractMonitoringSystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ContractMonitoringSystem.Repositories;
using ContractMonitoringSystem.Models.ViewModel;
using ContractMonitoringSystem.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace ContractMonitoringSystem.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepo;
        private readonly IBaseRepository<UserApp> _baseRepo;
        private string _accountId = "UserId";
        private readonly BaseController _baseController;

        public AccountController(ApplicationDbContext context, UserRepository userRepo, IBaseRepository<UserApp> baseRepo, BaseController baseController)
        {
            _userRepo = userRepo;
            _context = context;
            _baseRepo = baseRepo;
            _baseController = baseController;
             
        }

        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(string username, string password)
        {
            var userFromDb = await _userRepo.GetUserDetails(username, password);

            if (userFromDb == null)
            {
                ViewBag.login = "Invalid Credentials";
                return View();
            }

            var department = userFromDb.Department ?? "";
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userFromDb.Username),
                    new Claim(ClaimTypes.Email, userFromDb.Email?? ""),
                    //new Claim("UserId", userFromDb.UserId.ToString())
                    
                    new Claim(_accountId, userFromDb.UserId.ToString())
                };
            //_baseController.userId = Convert.ToInt32(_accountId);
            if (IsRoleValid(department))
            {
                claims.Add(new Claim(ClaimTypes.Role, department));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                if(department=="Planning") return RedirectToAction("Index", "SummaryDashboard");
                return RedirectToAction("Index", "SiteInstruction");
            }

            return View();
        }

        private bool IsRoleValid(string department)
        {
            string[] validRoles = { "Admin", "Cost", "Office", "Manager", "Mamplasan", "Planning", "Engineering", "Audit", "LYE", "MTYE", "Management" };
            return validRoles.Contains(department);
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear(); // Clear session data
            HttpContext.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            var ProjectList = _context.ProjectDetails
                .Select(p => new{p.Id, p.ProjectName}).ToList();
            ViewBag.ProjectList = ProjectList;
            return View();
        }
        public async Task<IActionResult> Register(UserApp userApp)
        {
            var ProjectList = _context.ProjectDetails
                .Select(p => new { p.Id, p.ProjectName }).ToList();
            ViewBag.ProjectList = ProjectList;
            if (userApp == null)
            {
                ViewBag.register = string.Format("Invalid Credentials");
                return View("CreateAccount", userApp);
            }
            else if (string.IsNullOrEmpty(userApp.Username) || string.IsNullOrEmpty(userApp.Email) || string.IsNullOrEmpty(userApp.Password) || string.IsNullOrEmpty(userApp.ConfirmPassword))
            {
                ViewBag.register = string.Format("Invalid Credentials");
                return View("CreateAccount", userApp);
            }
            if ((userApp.Password !=userApp.ConfirmPassword))
            {
                ViewBag.register = string.Format("Password Not Match!");
                return View("CreateAccount", userApp);
            }
            if (userApp != null)
            {
                if (!string.IsNullOrEmpty(userApp.Username))
                {
                    var checkName = _context.UserApps
                        .Where(l => l.Username == userApp.Username)
                        .ToList();

                    if (checkName.Count > 0)
                    {
                        ViewBag.register = "Username already taken, Try another one";
                        return View("CreateAccount", userApp);
                    }
                }
                //.Insert(userApp);
                //_repo.Save();
                await _baseRepo.Create(userApp);
                ViewBag.register = ("new user successfully registered!");
                InsertUserProjects(userApp);

                return View("CreateAccount");
            }
           // userApp.ProjectList = _dbContext.Projects.ToList();
            return View("Home");
        }
        public void InsertUserProjects(UserApp userApp)
        {
            if (userApp.ProjId != null)
            {

                foreach (var projectid in userApp.ProjId)
                {

                    var userProjects = new UserProject()
                    {
                        ProdId = projectid,
                        UserId = userApp.UserId,
                    };
                    _context.Add(userProjects);
                    _context.SaveChanges();
                }
            }
        }
        public IActionResult CreateProject()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProject(ProjectDetail listProject)
        {
            if (string.IsNullOrEmpty(listProject.ProjectName) || listProject == null)
            {
                ViewBag.success = "Failed in Creating a Project";
                return View("CreateProject");
            }
            if (listProject != null)
            {
                var userProjects = new UserProject();
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                var UserId = int.TryParse(userIdClaim?.Value, out int userId) ? userId : 0;
                var checkProjectName =  _context.ProjectDetails
                                .Where(l => l.ProjectName == listProject.ProjectName)
                                .ToList();
                if (checkProjectName.Count == 0)
                {
                    _context.ProjectDetails.Add(listProject);
                    _context.SaveChanges();
                    ViewBag.success = "Successfully Added a Project";
                    return View("CreateProject");
                }
                if (checkProjectName.Count > 0)
                {
                    ViewBag.success = "Failed in Adding a Project, Project Name already exist!";
                    return View("CreateProject");
                }
            }
            ViewBag.success = "Failed in Creating a Project";
            return View("CreateProject");
        }
    }
}

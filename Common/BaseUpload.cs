//using ContractMonitoringSystem.Controllers;
//using ContractMonitoringSystem.Data;
//using ContractMonitoringSystem.Interfaces;
//using ContractMonitoringSystem.Models;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace ContractMonitoringSystem.Common
//{
//    public class BaseUpload<T>: IBaseUpload<T> where T : class
//    {
//        private readonly IBaseRepository<FileUpload> _fileRepo;
//        private string _path = "wwwroot/Files/";
//        private string _folderName = "";
//        private string Site = "SiteInstruction";
//        private string Pot = "SiteInstruction";
//        private string EOT = "EOTClaim";
//        private string bill = "Billing";

//        public BaseUpload(ApplicationDbContext db, IBaseRepository<T> baseRepo, IBaseRepository<FileUpload> fileRepo, BaseController baseController)
//        {
//            _fileRepo = fileRepo;
//        }
//        public async Task<T> UploadFile(T t, IList<IFormFile> files, int? userid, int id)
//        {
//            if (t != null && files != null && files.Count > 0)
//            {
//                var getType = t.GetType();
//                foreach (var file in files)
//                {
//                    if (file.Length > 0)
//                    {
//                        if (getType.Name == Site) _folderName = Site;
//                        else if (getType.Name == Pot) _folderName = Pot;
//                        else if (getType.Name == EOT) _folderName = EOT;
//                        else _folderName = bill;
//                        string path = Path.Combine(Directory.GetCurrentDirectory(), _path + _folderName);
//                        if (!Directory.Exists(path))
//                            Directory.CreateDirectory(path);
//                        string fileNameWithPath = Path.Combine(path, file.FileName);

//                        // Save the file to disk
//                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
//                        {
//                            await file.CopyToAsync(stream);
//                        }

//                        FileUpload fileUpload = new FileUpload
//                        {
//                            FileName = file.FileName,
//                            ContentType = file.ContentType,
//                            Size = file.Length,
//                            FilePath = path,
//                            UserId = userid,
//                            CMSiteId = getType.Name == "SiteInstruction" ? id : null,
//                            PotId = getType.Name == "PotentialClaim" ? id : null,
//                        };
//                        using (var memoryStream = new MemoryStream())
//                        {
//                            await file.CopyToAsync(memoryStream);
//                            fileUpload.FileContent = memoryStream.ToArray();
//                        }
//                        await _fileRepo.Create(fileUpload);
//                    }
//                }
//            }
//            return default;

//        }
//    }
//}


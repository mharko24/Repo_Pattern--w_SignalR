using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ContractMonitoringSystem.Data
{
    public class UserApp
    {
        [Required]
        [Key]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string? Department { get; set; }

        public string? Email { get; set; }

        public string? ProjectName { get; set; }
        [NotMapped]
        public int[]? ProjId { get; set; }
        //[NotMapped]
        //public IList<UserProject> userProjects { get; set; }
        //[NotMapped]
        //public virtual ICollection<Project>[]? ProjectList { get; set; }

        [NotMapped]
        public string? txtProjectName { get; set; }
        [NotMapped]
        public string? ConfirmPassword { get; set; }
        [NotMapped]
        public bool checkProjectname { get; set; }
    }
}

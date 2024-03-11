using ContractMonitoringSystem.Models.ViewModel;

namespace ContractMonitoringSystem.Interfaces
{
    public interface IGetUserRepository
    {
         List<ProjectCodeAndNameVM> CostProjectAndCode(int id);
         List<ProjectCodeAndNameVM> ProjectNameAndCodeList();
    }
}

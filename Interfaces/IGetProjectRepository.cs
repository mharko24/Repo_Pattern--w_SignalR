using ContractMonitoringSystem.Models.ViewModel;

namespace ContractMonitoringSystem.Interfaces
{
    public interface IGetProjectRepository
    {
        ProjectCodeAndNameVM CostProjectCodeAndName(int id);
        List<ProjectCodeAndNameVM> EngProjectNameAndCode(int id);
        List<ProjectCodeAndNameVM> AdminProjectNameAndCode();
    }
}

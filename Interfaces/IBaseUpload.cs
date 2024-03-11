namespace ContractMonitoringSystem.Interfaces
{
    public interface IBaseUpload<T>
    {
        Task<T> UploadFile(T t, IList<IFormFile> files, int? userid, int id);
    }
}

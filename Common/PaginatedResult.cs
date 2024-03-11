namespace ContractMonitoringSystem.Common
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T>? data { get; set; }
        public int pageCurrent { get; set; }
        public int numSize { get; set; }
        public string? Keyword { get; set; }
    }
}

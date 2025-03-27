namespace ClientRegistry.MVC.Models
{
    public class PagedClientViewModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public string? Search { get; set; }
        public IEnumerable<ClientViewModel> Clients { get; set; }
    }
}

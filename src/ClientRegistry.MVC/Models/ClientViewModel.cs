namespace ClientRegistry.MVC.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public DateTime RegisterDateTime { get; set; }
    }
}

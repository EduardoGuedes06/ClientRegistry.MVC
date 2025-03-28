using System.ComponentModel.DataAnnotations;

namespace ClientRegistry.MVC.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public string Document { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public DateTime RegisterDateTime { get; set; }
    }
}

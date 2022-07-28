using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SchoolRectorCoordinator
    {
        public int SchoolId { get; set; }

        public string SchoolName { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserId { get; set; }
    }
}

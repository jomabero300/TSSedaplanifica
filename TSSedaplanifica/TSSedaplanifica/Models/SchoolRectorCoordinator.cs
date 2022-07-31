using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SchoolRectorCoordinator
    {
        public int SchoolId { get; set; }

        [Display(Name = "Institución educativa")]
        public string SchoolName { get; set; }

        public string rol { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string UserId { get; set; }

        [Display(Name = "Sede")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sede.")]
        public int SchoolCampus { get; set; }

        [Display(Name = "Fecha de ingreso")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime HireOfDate { get; set; }

        [Display(Name = "Fecha de retiro")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [DataType(DataType.Date)]
        public DateTime EndOfDate { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Models
{
    public class SchoolViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Institución educativa")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Codigo dane")]
        [MaxLength(18, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DaneCode { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una clase de categoría.")]
        public int CityId { get; set; }

        [Display(Name = "Zona")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una clase de categoría.")]
        public int ZoneId { get; set; }

        public School SchoolCampus { get; set; }
    }
}

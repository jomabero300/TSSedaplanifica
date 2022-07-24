using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Categoría")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Clase de categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una clase de categoría.")]
        public int CategoryTypeId { get; set; }

    }
}

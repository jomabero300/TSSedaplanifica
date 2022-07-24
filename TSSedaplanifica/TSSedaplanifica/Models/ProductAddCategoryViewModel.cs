using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class ProductAddCategoryViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoryId { get; set; }

        [Display(Name = "Clase de Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoryTypeId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}

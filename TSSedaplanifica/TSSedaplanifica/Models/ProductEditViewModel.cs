using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Models
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(140, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; }

        [Display(Name = "Unidad de medida")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una clase de categoría.")]
        public IEnumerable<SelectListItem> MeasureUnit { get; set; }
        //public MeasureUnit MeasureUnit { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SolicitDetailViewModel
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
        public int MeasureUnit { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }
    }
}

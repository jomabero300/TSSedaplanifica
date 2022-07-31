using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SolicitDetailProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Clases de categorías")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una clase de categoría.")]
        public int CategoryTypeId { get; set; }

        [Display(Name = "Categorías")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría.")]

        public int CategoryId { get; set; }

        [Display(Name = "Elemento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un elemento.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ProductId { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe ingresar un valor superior a cero.")]
        public float Quantity { get; set; }

    }
}

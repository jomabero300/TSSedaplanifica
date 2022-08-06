using System.ComponentModel.DataAnnotations;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Models
{
    public class SolicitConsolidateDetailsViewModel
    {
        [Display(Name = "Elemento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Product Product { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(0, float.MaxValue, ErrorMessage = "Debe ingresar un valo superios a {0}.")]
        public float Quantity { get; set; }
    }
}

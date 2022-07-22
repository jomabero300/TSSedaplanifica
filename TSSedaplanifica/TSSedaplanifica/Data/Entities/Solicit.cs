using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Solicit", Schema = "Seda")]
    public class Solicit
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Institución")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public School School { get; set; }

        [Display(Name = "Fecha solicitud")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfSolicit { get; set; } = DateTime.Now;

        [Column(TypeName = "varchar(200)")]
        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public SolicitState SolicitStates { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSSedaplanifica.Enum;

namespace TSSedaplanifica.Data.Entities
{
    [Table("SolicitState", Schema = "Seda")]
    public class SolicitState
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(40)")]
        [Display(Name = "Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
    }
}

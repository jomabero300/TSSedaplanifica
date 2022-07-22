using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Zones", Schema = "Gene")]
    public class Zone
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Zona")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
    }
}

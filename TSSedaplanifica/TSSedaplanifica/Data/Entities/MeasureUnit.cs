using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("MeasureUnits", Schema = "Gene")]
    public class MeasureUnit
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Unidad de medida")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(4)")]
        [Display(Name = "Prefijo")]
        [MaxLength(4, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NameShort { get; set; }

    }
}

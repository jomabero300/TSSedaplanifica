using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("CategoryTypes", Schema = "Seda")]
    public class CategoryType
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Categoria")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        public ICollection<CategoryTypeDer> CategoryTypeDers { get; set; }

    }
}
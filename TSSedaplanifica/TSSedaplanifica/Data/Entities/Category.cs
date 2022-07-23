using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Categories", Schema = "Seda")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Categoría")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Display(Name = "Tipo")]
        [NotMapped]       
        public ICollection<CategoryTypeDer> CategoryTypeDers { get; set; }

        [NotMapped]       
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}

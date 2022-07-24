using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("ProductCategories", Schema = "Seda")]
    public class ProductCategory
    {
        [Key]
        
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Category Category { get; set; }

        [Display(Name = "Elemento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Product Product { get; set; }
    }
}

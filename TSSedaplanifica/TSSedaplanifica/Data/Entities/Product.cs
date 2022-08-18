using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Products", Schema = "Seda")]
    public class Product
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(250)")]
        [Display(Name = "Nombre")]
        [MaxLength(140, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(500)")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Description { get; set; }

        [Display(Name = "Unidad de medida")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public MeasureUnit MeasureUnit { get; set; }

        [Display(Name = "Elemento")]
        public string FullName => $"{Name} {Description}";


        public ICollection<ProductCategory> ProductCategories { get; set; }

        [Display(Name = "Categorías")]
        public int CategoriesNumber => ProductCategories == null ? 0 : ProductCategories.Count;

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("CategoryTypeDers", Schema = "Seda")]
    public class CategoryTypeDer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public CategoryType CategoryType { get; set; }
    }
}

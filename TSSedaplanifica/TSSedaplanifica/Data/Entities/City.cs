using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Cities", Schema = "Gene")]
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Ciudad")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        //[JsonIgnore]
        public State State { get; set; }

        //public ICollection<User> Users { get; set; }
    }
}

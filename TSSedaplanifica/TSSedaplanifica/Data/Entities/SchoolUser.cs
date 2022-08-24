using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("SchoolUsers", Schema = "Seda")]
    public class SchoolUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public School School { get; set; }
        [Required]

        public ApplicationUser ApplicationUser { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        [MaxLength(450, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required]
        public string ApplicationRole { get; set; }
        [Required]
        public DateTime HireOfDate { get; set; }
        [Required]
        public DateTime EndOfDate { get; set; }
        [Required]
        public bool isEnable { get; set; }
    }
}

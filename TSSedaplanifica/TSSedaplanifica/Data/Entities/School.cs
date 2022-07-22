using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Schools", Schema = "Seda")]
    public class School
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Institución educativa")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(18)")]
        [Display(Name = "Codigo dane")]
        [MaxLength(18, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DaneCode { get; set; }

        [Column(TypeName = "varchar(200)")]
        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public City City { get; set; }

        [Display(Name = "Zona")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Zone Zone { get; set; }
        public ICollection<SchoolImage> SchoolImages { get; set; }
    }
}

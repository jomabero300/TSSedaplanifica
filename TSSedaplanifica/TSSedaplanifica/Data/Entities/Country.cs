using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Countries", Schema = "Gene")]
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "País")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<State> States { get; set; }

        [Display(Name = "Departamentos")]
        public int StatesNumber => States == null ? 0 : States.Count;

        [Display(Name = "Ciudades")]
        public int CitiesNumber => States == null ? 0 : States.Sum(s => s.CityNumber);

    }
}

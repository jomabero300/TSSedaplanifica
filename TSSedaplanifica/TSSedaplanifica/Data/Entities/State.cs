using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("States", Schema = "Gene")]
    public class State
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Departamento/Estado")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
        //[JsonIgnore]
        public Country Country { get; set; }

        public ICollection<City> Cities { get; set; }

        [Display(Name = "Ciudades")]
        public int CityNumber => Cities == null ? 0 : Cities.Count;
    }
}

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("Solicits", Schema = "Seda")]
    public class Solicit
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Institución")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public School School { get; set; }

        [Display(Name = "Fecha solicitud")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfSolicit { get; set; } = DateTime.Now;

        [Column(TypeName = "varchar(200)")]
        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public SolicitState SolicitStates { get; set; }

        [Display(Name = "Fecha fecibido")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfReceived { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que recibe")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ApplicationUser UserReceived { get; set; }

        [Display(Name = "Fecha Aprobado/Denegado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfApprovedDenied { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que Aprobado/Denegado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ApplicationUser UserApprovedDenied { get; set; }

        [Display(Name = "Fecha cierre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfClosed { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que cierra")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ApplicationUser UserClosed { get; set; }

        public Solicit SolicitReferred { get; set; }

        [JsonIgnore]

        public ICollection<SolicitDetail> SolicitDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Elementos")]
        public int ProductCount => SolicitDetails == null ? 0 : SolicitDetails.Count;


    }
}

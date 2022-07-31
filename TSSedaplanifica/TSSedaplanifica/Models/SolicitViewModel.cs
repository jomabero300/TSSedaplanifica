using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SolicitViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Institución")]
        public int SchoolId { get; set; }

        [Display(Name = "Fecha solicitud")]
        public DateTime DateOfSolicit { get; set; } = DateTime.Now;

        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int SolicitStatesId { get; set; }

        [Display(Name = "Fecha fecibido")]
        public DateTime DateOfReceived { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que recibe")]
        public string UserReceivedId { get; set; }

        [Display(Name = "Fecha Aprobado/Denegado")]
        public DateTime DateOfApprovedDenied { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que Aprobado/Denegado")]
        public string UserApprovedDeniedId { get; set; }

        [Display(Name = "Fecha cierre")]
        public DateTime DateOfClosed { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que cierra")]
        public string UserClosedId { get; set; }

        public int SolicitReferredId { get; set; }

    }
}

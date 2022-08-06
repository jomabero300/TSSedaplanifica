using System.ComponentModel.DataAnnotations;
using TSSedaplanifica.Data.Entities;

namespace TSSedaplanifica.Models
{
    public class SolicitConsolidateViewModel
    {
        [Display(Name = "Institución")]
        public School School { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        public List<SolicitConso> SolicitCons { get; set; }=new List<SolicitConso>();

        public List<SolicitConsolidateDetailsViewModel> Details { get; set; }=new List<SolicitConsolidateDetailsViewModel>(){ };
    }

    public class SolicitConso
    {
        public int id { get; set; }
    }
}

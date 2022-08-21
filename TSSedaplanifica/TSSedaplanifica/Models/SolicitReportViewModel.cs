using System.ComponentModel.DataAnnotations;

namespace TSSedaplanifica.Models
{
    public class SolicitReportViewModel
    {

        [Display(Name = "Municipio")]
        public int CityId { get; set; }

        [Display(Name = "Institución")]
        public int SchoolId { get; set; }

        [Display(Name = "Sede")]
        public int CampusId { get; set; }

        [Display(Name = "Clase categoría")]
        public int CategoryTypeId { get; set; }

        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Display(Name = "Elemento")]
        public int ProductId { get; set; }

        [Display(Name = "Desde")]
        public DateTime DateOfFrom { get; set; }

        [Display(Name = "Hasta")]
        public DateTime DateOfTo { get; set; }
    }
}

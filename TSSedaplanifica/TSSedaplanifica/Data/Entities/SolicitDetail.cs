using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("SolicitDetails", Schema = "Seda")]
    public class SolicitDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Solicit Solicit { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Product Product { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float DirectorQuantity { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float PlannerQuantity { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float DeliveredQuantity { get; set; }

        [Column(TypeName = "varchar(200)")]
        [Display(Name = "Descripción")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; }

        [Display(Name = "Fecha entrega")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime DateOfClosed { get; set; } = DateTime.Now;

        [Display(Name = "Usuario que entrega")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public ApplicationUser UserDelivered { get; set; }

        //[JsonIgnore]
        //public ICollection<Product> Products { get; set; }
    }
}

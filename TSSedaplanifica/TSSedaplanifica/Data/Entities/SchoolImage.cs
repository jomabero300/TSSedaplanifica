using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSSedaplanifica.Data.Entities
{
    [Table("SchoolImages", Schema = "Seda")]
    public class SchoolImage
    {
        [Key]
        public int Id { get; set; }

        public School School { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7266/images/noimage.png"
            : $"https://localhost:7266/images/Schools/{ImageId}.png";

    }
}

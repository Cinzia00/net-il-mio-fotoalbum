using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome della categoria è obbligatorio!")]
        [StringLength(50, ErrorMessage = "Il nome della categoria non può superare i 50 caratteri")]
        public string Name { get; set; }

        public Category() { }


        public List<Photo>? Photos { get; set; }

    }
}

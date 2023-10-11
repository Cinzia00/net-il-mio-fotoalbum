using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo della foto è obbligatorio")]
        [MaxLength(100, ErrorMessage = "La lunghezza massima è di 100 caratteri")]
        public string Title { get; set; }


        [Column(TypeName = "text")]
        [Required(ErrorMessage = "La descrizione della foto è obbligatoria!")]
        public string Description { get; set; }


        [Url(ErrorMessage = "Devi inserire un link valido ad un'immagine")]
        [MaxLength(500, ErrorMessage = "Il link non può essere lungo più di 500 caratteri")]
        public string Image { get; set; }

        public bool Visible { get; set; }

        public Photo() { }

        public Photo(string title, string description, string image, bool visible)
        {
            Title = title;
            Description = description;
            Image = image;
            Visible = visible;
        }
        public List<Category>? Categories { get; set; }



    }
}

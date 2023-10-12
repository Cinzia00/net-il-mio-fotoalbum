using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Contact
    {

        public int Id { get; set; }

        [Required]
        public string Email { get; set; }


        [Column(TypeName = "text")]
        [Required(ErrorMessage = "Il testo del messaggio è obbligatorio!")]
        [MaxLength(300, ErrorMessage = "La lunghezza massima è di 300 caratteri")]
        public string Message { get; set; }



        public Contact() { }
        public Contact(string email, string message)
        {
            Email = email;
            Message = message;
        }

    }
}

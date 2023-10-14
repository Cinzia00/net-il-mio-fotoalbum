using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics.Contracts;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPhotos()
        {
            using (PhotoContext db = new PhotoContext())
            {
                List<Photo> photos = db.Photos.ToList();

                return Ok(photos);
            }
        }


        [HttpGet]
        public IActionResult SearchByTitle(string search)
        {
            using (PhotoContext db = new PhotoContext())
            {

                if(search == null)
                {
                    return BadRequest(new { Message = "Non è stata trovata nessuna foto!" });
                }
                else
                {
                    List<Photo> photos = db.Photos.Where(photo => photo.Title.ToLower().Contains(search.ToLower())).ToList();
                    return Ok(photos);
                }
            }
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? detailPhoto = db.Photos.Where(photo => photo.Id == id).Include(p => p.Categories).FirstOrDefault();

                if (detailPhoto == null)
                {
                    return NotFound("La foto non è stata trovata");
                }
                else
                {
                    return Ok(detailPhoto);
                }
            }
        }


        [HttpPost]
        public IActionResult SendMessage([FromBody] Contact data)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Contact newContact = new Contact(data.Email, data.Message);

                db.Contact.Add(newContact);
                db.SaveChanges();

                return Ok("Messaggio inviato correttamente");
            }

        }
    }
}

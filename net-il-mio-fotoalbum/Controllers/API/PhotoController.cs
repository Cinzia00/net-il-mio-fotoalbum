using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPhotos()
        {
            using (PhotoContext db = new PhotoContext())
            {
                List<Photo> photos = db.Photos.Include(c => c.Categories).ToList();

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
    }
}

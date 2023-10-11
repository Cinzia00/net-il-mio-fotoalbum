using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            using (PhotoContext db = new PhotoContext())
            {
                List<Photo> photos = db.Photos.ToList();

                return View (photos);
            }
        }

        public IActionResult searchByTitle(string searchByName)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? photos = db.Photos.Where(photo => photo.Title == searchByName).FirstOrDefault();

                return View(photos);
            }
        }


        [HttpGet]
        public IActionResult Create()
        {
            using (PhotoContext db = new PhotoContext())
            {
                List<Category> categories = db.Categories.ToList();
                List<SelectListItem> categoryList = new List<SelectListItem>();

                foreach(Category category in categories)
                {
                    categoryList.Add(new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
                }

                PhotoFormModel model = new PhotoFormModel();
                model.Photos = new Photo();
                model.Categories = categoryList;

                return View("Create", model);
            }

        }

        [HttpPost]
        public IActionResult Create(PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PhotoContext db = new PhotoContext())
                {
                    List<SelectListItem> categoryList = new List<SelectListItem>();
                    List<Category> databasecategoryList = db.Categories.ToList();

                    foreach (Category category in databasecategoryList)
                    {
                        categoryList.Add(
                            new SelectListItem
                            {
                                Text = category.Name,
                                Value = category.Id.ToString()
                            });
                    }

                    data.Categories = categoryList;
                    return View("Create", data);
                }
            }

            data.Photos.Categories = new List<Category>();

            using (PhotoContext db = new PhotoContext())
            {
                if (data.SelectedCategoryId != null)
                {
                    foreach (string selectedCategory in data.SelectedCategoryId)
                    {
                        int intSelectedCategory = int.Parse(selectedCategory);
                        Category? categoryDb = db.Categories.Where(c => c.Id == intSelectedCategory).FirstOrDefault();

                        data.Photos.Categories.Add(categoryDb);
                    }
                }

                db.Photos.Add(data.Photos);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

        }

    }
}

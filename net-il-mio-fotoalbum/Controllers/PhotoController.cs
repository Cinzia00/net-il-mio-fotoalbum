using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN,USER")]
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



        public IActionResult Details(int id)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? detailPhoto = db.Photos.Where(photo => photo.Id == id).Include(p => p.Categories).FirstOrDefault();

                if(detailPhoto == null)
                {
                    return NotFound("La foto non è stata trovata");
                }else
                {
                    return View("Details", detailPhoto);
                }
            }
        }



        [Authorize(Roles = "ADMIN")]
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



        [Authorize(Roles = "ADMIN")]
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

                this.SetImageFileFromFormFile(data);

                db.Photos.Add(data.Photos);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? photoToEdit = db.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

                if (photoToEdit == null)
                {
                    return NotFound("La pizza che vuoi modificare non è stata trovata");
                }
                else
                {
                    List<Category> categories = db.Categories.ToList();
                    List<SelectListItem> categoryList = new List<SelectListItem>();

                    foreach (Category category in categories)
                    {
                        categoryList.Add(
                            new SelectListItem
                            {
                                Text = category.Name,
                                Value = category.Id.ToString(),
                                Selected = photoToEdit.Categories.Any(associatedCategory => associatedCategory.Id == category.Id)
                            });
                    }

                    PhotoFormModel model = new PhotoFormModel { Photos = photoToEdit, Categories = categoryList };

                    return View("Update", model);
                }
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using (PhotoContext db = new PhotoContext())
                {
                    List<Category> categoriesDb = db.Categories.ToList();
                    List<SelectListItem> selectedCategory= new List<SelectListItem>();

                    foreach (Category categories in categoriesDb)
                    {
                        selectedCategory.Add(
                            new SelectListItem
                            {
                                Text = categories.Name,
                                Value = categories.Id.ToString(),
                            });
                    }
                    return View("Update", data);
                }
            }

            using (PhotoContext db = new PhotoContext())
            {
                Photo? photoToEdit = db.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();

                if (photoToEdit != null)
                {
                    photoToEdit.Title = data.Photos.Title;
                    photoToEdit.Description = data.Photos.Description;
                    photoToEdit.Image = data.Photos.Image;

                    if (data.SelectedCategoryId != null)
                    {
                        photoToEdit.Categories.Clear();

                        foreach (string categoryId in data.SelectedCategoryId)
                        {
                            int intselectedCategory = int.Parse(categoryId);

                            Category?categorynDb = db.Categories.Where(c => c.Id == intselectedCategory).FirstOrDefault();

                            if (categorynDb != null)
                            {
                                photoToEdit.Categories.Add(categorynDb);
                            }
                        }
                    }


                    if (data.ImageFormFile != null)
                    {
                        MemoryStream stream = new MemoryStream();
                        data.ImageFormFile.CopyTo(stream);
                        photoToEdit.ImageFile = stream.ToArray();
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                {
                    return NotFound("Non è stata trovata la foto da modificare");
                }

            }

        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? photoToDelete = db.Photos.Where(p => p.Id == id).FirstOrDefault();

                if (photoToDelete != null)
                {
                    db.Photos.Remove(photoToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La foto da eliminare non è stata trovata!");
                }
            }
        }


        private void SetImageFileFromFormFile(PhotoFormModel formData)
        {
            if (formData.ImageFormFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formData.ImageFormFile.CopyTo(stream);
            formData.Photos.ImageFile = stream.ToArray();

        }


    }
}

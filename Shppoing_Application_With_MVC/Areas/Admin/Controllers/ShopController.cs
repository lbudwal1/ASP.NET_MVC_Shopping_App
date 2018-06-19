using PagedList;
using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Shppoing_Application_With_MVC.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/categories
        public ActionResult Categories()
        {
            //delcare list of model
            List<CategoryVM> categoryVMList;

            // INit the list
            using (Db db = new Db())
            {
                categoryVMList = db.category.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

                //return view with list
                return View(categoryVMList);
        }

        [HttpPost]
        // Post: Admin/Shop/AddNewCategory
        public string AddNewCategory(string catName)
        {
            //declare id
            string id;

            using (Db db = new Db())
            {
                // check that category name is unique
                if(db.category.Any(x => x.Name == catName))
                
                    return "titletaken";
                

                // Init DTO
                CategoryDTO dto = new CategoryDTO();

                // Add to DTO
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                // save DTO
                db.category.Add(dto);
                db.SaveChanges();

                // get the id
                id = dto.Id.ToString();
            }

            return id;
            // return id
        }

        //Post :Admin/shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;
                // delcare page DTO
                CategoryDTO dto;

                //set sorting for each category
                foreach (var catId in id)
                {
                    dto = db.category.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }

        //Get :Admin/shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {

                // get the category
                CategoryDTO dto = db.category.Find(id);

                // remove the page
                db.category.Remove(dto);

                // save
                db.SaveChanges();
            }
            //redirect
            return RedirectToAction("categories");
        }


        //Post :Admin/shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id )
        {

            using (Db db = new Db())
            {
                // check category name is unique
                if (db.category.Any(x => x.Name == newCatName))
                    return "titletaken";

                // get dto
                CategoryDTO dto = db.category.Find(id);

                // edit dto

                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();

                // save
                db.SaveChanges();
            }
            //return
            return "OK!";

        }

        //Get :Admin/shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            //Init Model
            ProductVM model = new ProductVM();

            // Add select list of categories to model
            using (Db db = new Db())
            {
                model.categories = new SelectList(db.category.ToList(), "Id", "Name");
            }

                // return view with model

                return View(model);
        }

        [HttpPost]
        //POST :Admin/shop/AddProduct
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {
                    model.categories = new SelectList(db.category.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            // Make sure product name is unique
            using (Db db = new Db())
            {
                if(db.product.Any(x => x.Name == model.Name))
                {
                    model.categories = new SelectList(db.category.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product is taken!");
                    return View(model);
                }
               
            }

            // Declare product id
            int id;

            // Init and save productDTO
            using (Db db = new Db())
            {
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Slug = model.Slug;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                CategoryDTO catDTO = db.category.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = catDTO.Name;

                db.product.Add(product);
                db.SaveChanges();
                // Get inserted Id
                id = product.Id;
            }


            //set tempdata message
            TempData["SM"] = "You have added a Product!";

            #region Upload Image

            // create nessary directories
            var oirignalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

           
            var pathString1 = Path.Combine(oirignalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(oirignalDirectory.ToString(), "Products\\" + id.ToString() );
            var pathString3 = Path.Combine(oirignalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs" );
            var pathString4 = Path.Combine(oirignalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery" );
            var pathString5 = Path.Combine(oirignalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs" );

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);


            // check file uploaded
            if (file != null && file.ContentLength > 0)

            {

                // get file extension
                string ext = file.ContentType.ToLower();

                // verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {

                        model.categories = new SelectList(db.category.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);

                    }
                }

                // Init Image name
                string imageName = file.FileName;


                // save image name to DTO
                using (Db db = new Db())
                {
                    ProductDTO dto = db.product.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();
                }

                // Set original and thumb image paths

                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // save original 
                file.SaveAs(path);

                // create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }

            #endregion

            // redirect
            return RedirectToAction("AddProduct");
        }

        //GET :admin/shop/Products
        public ActionResult Products(int? page, int? catId)
        {
            // serach on google pagedlist and open github the first result on google 
            //delcare list of productVM
            List<ProductVM> listOfProductVM;

            //set page number
            var pageNumber = page ?? 1;


            using (Db db = new Db())
            {
                //Init the list
                listOfProductVM = db.product.ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                    .Select(x => new ProductVM(x))
                    .ToList();

                //Populate categories select list
                ViewBag.category = new SelectList(db.category.ToList(), "Id", "Name");

                // set selected category
                ViewBag.SelectedCat = catId.ToString();
               
            }

            // set pagination
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            
            ViewBag.OnePageOfProducts = onePageOfProducts;
            //return view with list
            return View(listOfProductVM);
        }
    }
}
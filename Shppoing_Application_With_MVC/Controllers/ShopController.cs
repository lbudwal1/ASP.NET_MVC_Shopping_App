using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shppoing_Application_With_MVC.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartical()
        {
            //delcare list of categoryVM
            List<CategoryVM> categoryVMList;

            //Init the list
            using (Db db = new Db())
            {
                categoryVMList = db.category.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

                //return the partical view
                return PartialView(categoryVMList);
        }

        //GET: /Shop/Category/name
        public ActionResult Category(string name)
        {
            //declare a list of ProductVM
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                //Get  category ID
                CategoryDTO categoryDTO = db.category.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                // Init the list
                productVMList = db.product.ToArray().Where(x => x.Category.Id == catId).Select(x => new ProductVM(x)).ToList();

                //Get category name
                var productCat = db.product.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }
                //return view with list
                return View(productVMList);
        }

        //GET: /shop/product-details/name
        //this is own create link of the action because we need (-) between product and details 
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //declare the vm and DTO
            ProductVM model;
            ProductDTO dto;

            // Init product id
            int id = 0;

            using (Db db = new Db())
            {

                // check if product exists
                if (! db.product.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                //Init ProductDTO
                dto = db.product.Where(x => x.Slug == name).FirstOrDefault();

                //Get id
                id = dto.Id;

                // Init Model
                model = new ProductVM(dto);
            }
            //Get gallery images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Thumbs")).Select(fn => Path.GetFileName(fn));

            //Return view with model
            return View("ProductDetails", model);
        }
    }
}
using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}
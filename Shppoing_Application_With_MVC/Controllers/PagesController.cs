using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shppoing_Application_With_MVC.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{page}
        public ActionResult Index(string page = "")
        {
            //get/Set page slug
            if (page == "")
                page = "home";

            //declare model and DTO
            PageVM model;
            PageDTO dto;

            //check if page exists
            using (Db db = new Db())
            {
                if (! db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            // get page DTO
            using (Db db = new Models.Data.Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            //set page title
            ViewBag.PageTitle = dto.Title;

             // check the slidebar
            if (dto.HasSlideBar == true)
            {
                ViewBag.Slidebar = "Yes";
            }
            else
            {
                ViewBag.Slidebar = "No";
            }

            // Init Model
            model = new PageVM(dto);

           // return view with model
           return View(model);
        }

        public ActionResult PagesMenupartical()
        {
            // Declare a list of PageVM
            List<PageVM> pageVMList;

            //Get all pages except home
            using (Db db = new Db())
            {
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }

            // Return partical view with list
            return PartialView(pageVMList);
        }

        public ActionResult SidebarPartial()
        {

            //declare model
            SidebarVM model;

            //Init Model
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Slidebar.Find(1);

                model = new SidebarVM(dto);
            }

                //return partical view with model
                return PartialView(model);
        }
    }
}
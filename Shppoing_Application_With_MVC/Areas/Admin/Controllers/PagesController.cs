using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shppoing_Application_With_MVC.Areas.Admin.Controllers
{

    /* x =>  is a lynda expretion where x is parameter and value after => is what return
     for eg = a function(x)
     return x.id != id;
    */

    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare list of PageVM
            List<PageVM> pageList;

            // In it the list
            using (Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //return the list
                return View(pageList);
        }

        //GET : Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        //Post : Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // check model state
            if(! ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {

                // delcare slug
                string slug;

                // In it PageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;

                // check for and set slug if need be
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // make sure title and slug for unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That Title or slug is already exist in system.");
                    return View(model);
                }

                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSlideBar = model.HasSlideBar;

                //here 100 logic is if we add a new page so it will be last page as in a sorting series
                dto.Sorting = 100;

                //save DTO
                db.Pages.Add(dto);
                db.SaveChanges();


            }

            // set TempDAta message
            TempData["SM"] = "You have successfully added a New Page";

            /* redirect is a method so once we successfully add a new page than 
            it return user to add new page screen automatically*/
            return RedirectToAction("AddPage");
        }

        //Get :Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // declare pagevm
            PageVM model;

            using (Db db = new Db())
            {

                //get the page and   id is primary key 
                PageDTO dto = db.Pages.Find(id);

            
                //confirm page exist
                if(dto == null)
                {
                    return Content("This Page does not exists");
                }

                // Init pageVm
                model = new PageVM(dto);

            }

                //return view with model
                return View(model);
        }


        //Post : Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //get page id
                int id = model.Id;

                // In it slug
                string slug = "home";

                // get the page
                PageDTO dto = db.Pages.Find(id);

                // dto the title
                dto.Title = model.Title;

                // check slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // make sure title and slug are unique
                if(db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                 db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug))
                    {
                    ModelState.AddModelError("", "That title or slug already exist in system.");
                }

                //dto rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSlideBar = model.HasSlideBar;

                // dto save changes
                db.SaveChanges();
            }
            // set tempdata message
            

            TempData["SM"] = "You Edited the Page.";
                // Redirect
            
                return RedirectToAction("Editpage");
        }

        //Get :Admin/Pages/PageDetail/id
        [HttpGet]
        public ActionResult PageDetail(int id)
        {
            // declare pagevm
            PageVM model;

            using (Db db = new Db())
            {

                //get the page and   id is primary key 
                PageDTO dto = db.Pages.Find(id);


                //confirm page exist
                if (dto == null)
                {
                    return Content("This Page does not exists");
                }

                // Init pageVm
                model = new PageVM(dto);

            }

            //return view with model
            return View(model);
        }

        //Get :Admin/Pages/deletepage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {

                // get the page
                PageDTO dto = db.Pages.Find(id);

                // remove the page
                db.Pages.Remove(dto);

                // save
                db.SaveChanges();
            }
                //redirect
                return RedirectToAction("Index");
        }

        //Post :Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;
                // delcare page DTO
                PageDTO dto;

                //set sorting for each page
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }

            }
        }

        [HttpGet]
        //Get :Admin/Pages/EditSidebar
        public ActionResult EditSidebar()
        {
            // declare model
            SidebarVM model;

            using (Db db = new Db())
            {


                // get the dto i put 1 beacuse i know it always be 1
                SidebarDTO dto = db.Slidebar.Find(1);
                // Init model
                model = new SidebarVM(dto);
            }
                // return view with model;

                return View(model);
        }

        [HttpPost]
        //Post :Admin/Pages/EditSidebar
        public ActionResult EditSidebar(SidebarVM model)

        {
            using (Db db = new Db())
            {


                // get dto
                SidebarDTO dto = db.Slidebar.Find(1);

                //dto the body
                dto.Body = model.Body;

                // save
                db.SaveChanges();

            }
            //set tempdata
            TempData["SM"] = "you Have edited the sidebar";

                //redirect
                return RedirectToAction("EditSidebar");
        }
    }
}
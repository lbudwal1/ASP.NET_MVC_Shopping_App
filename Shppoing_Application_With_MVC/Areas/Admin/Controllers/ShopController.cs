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
    }
}
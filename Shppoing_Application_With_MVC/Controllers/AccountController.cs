using Shppoing_Application_With_MVC.Models.Data;
using Shppoing_Application_With_MVC.Models.ViewModel.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shppoing_Application_With_MVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        //GET: /account/login
        [HttpGet]
        public ActionResult Login()
        {
            //Confirm user is not logged in
            // Identity is not i create its already exist in the framework which helps Name property to Identitfy User in the system
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
                return RedirectToAction("user-profile");

            //Return View
            return View();
        }

        //GET: /account/login
        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            //Check model state
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            //Check if the User is valid

            bool isValid = false;

            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }
            if (! isValid)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

           else
            {
                FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
            }


        }


        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {

            return View("CreateAccount");
        }


        // POST: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", model);
            }

            //check if password match
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password Does not Match.");
                return View("CreateAccount", model);
            }

            using (Db db = new Db())
            {

                //make sure username is unique
                if (db.Users.Any(x => x.Username.Equals(model.Username)))
                {
                    ModelState.AddModelError("", "Username " + model.Username + "is taken.");
                    model.Username = "";
                    return View("CreateAccount", model);
                }

                //create userDTO
                UserDTO userDTO = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Username = model.Username,
                    Password = model.Password
                };

                //Add the DTO
                db.Users.Add(userDTO);

                //Save
                db.SaveChanges();

                //Add to userRoleDTO
                int id = userDTO.Id;

                UserRoleDTO userRolesDTO = new UserRoleDTO()
                {
                    UserId = id,
                    RoleId = 2
                };

                db.UserRole.Add(userRolesDTO);
                db.SaveChanges();

            }

            // Create a TempData Message
            TempData["SM"] = "Your Account Successfully Created.";

            // Redirect

            return Redirect("~/account/login");
        }

        //GET: /account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }

        public ActionResult UserNavPartial()
        {
            //Get username
            string username = User.Identity.Name;

            //declare model
            UserNavPartialVM model;

            using (Db db = new Db())
            {
                //Get the user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);

                //Build the model
                model = new UserNavPartialVM()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

            }
            //return partical view with model
            return PartialView(model);
        }

        //GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            //Get username
            string username = User.Identity.Name;

            // Declare model
            UserProfileVM model;

            using (Db db = new Db())
            {
                // Get user
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);

                // Build Model
                model = new UserProfileVM(dto);

            }
            //Return View with model 
            //here i specify the UserProfile because this is action name in top
            return View("UserProfile", model);
        }

        //POST: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            //Chek model state
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            // check if password match if need be
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if(!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password Does not Match.");
                    return View("UserProfile", model);
                }
            }

            using (Db db = new Db())
            {
                //Get username
                string username = User.Identity.Name;

                // Make sure username is unique
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                {
                    ModelState.AddModelError("", "Username " + model.Username + " Already Exist in System.");
                    model.Username = "";
                    return View("UserProfile", model);
                }

                // Edit DTO
                UserDTO dto = db.Users.Find(model.Id);

                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAddress = model.EmailAddress;
                dto.Username = model.Username;

                if(!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }

                // Save
                db.SaveChanges();
            }

            // Set TempData Message
            TempData["SM"] = "You have Successfully Edit your Profile.";

                //Redirect
                return Redirect("~/account/user-profile");
        }
    }
}
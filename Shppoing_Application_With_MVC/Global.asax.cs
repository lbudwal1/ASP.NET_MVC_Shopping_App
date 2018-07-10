﻿using Shppoing_Application_With_MVC.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shppoing_Application_With_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //this one enable user roles in our application
        //which helps to display user profile with name in the top when user login and if admin login then it shows nothing
        protected void Application_AuthenticateRequest()
        {
            //Check if user is logged in
            if (User == null)
            {
                return;
            }

            // Get username
            //here is add Context because we want to update User in the end, it helps to update
            string username = Context.User.Identity.Name;

            //Declare array of roles
            string[] roles = null;

            using (Db db = new Db())
            {
                //Populate roles
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);

                roles = db.UserRole.Where(x => x.UserId == dto.Id).Select(x => x.Role.Name).ToArray();

            }

            // Build IPrincipal object
            IIdentity userIdentity = new GenericIdentity(username);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

            //Update Context.User
            Context.User = newUserObj;
        }
    }
}

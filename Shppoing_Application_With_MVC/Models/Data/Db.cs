using Shppoing_Application_With_MVC.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Shppoing_Application_With_MVC.Models.Data
{
    /*this is a gateway to our database this class and properties DBContext and Dbset helps 
    to convert our request to a sqlquery and do changes  in database  */
    public class Db : DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }

        public DbSet<SidebarDTO> Slidebar { get; set; }

        public DbSet<CategoryDTO> category { get; set; }
    }
}
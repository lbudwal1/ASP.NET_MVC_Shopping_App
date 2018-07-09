using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shppoing_Application_With_MVC.Models.Data
{

    [Table("tbleCategory")]
    public class CategoryDTO
    {
        public int Id { get; set; }
        public  string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}
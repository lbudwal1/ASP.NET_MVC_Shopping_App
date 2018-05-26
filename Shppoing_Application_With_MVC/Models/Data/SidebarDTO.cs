using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Shppoing_Application_With_MVC.Models.Data
{
    [Table("tblSlidebar")]
    public class SidebarDTO
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }

    }
}
using Shppoing_Application_With_MVC.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shppoing_Application_With_MVC.Models.ViewModel.Pages
{

    /* this class we can create in a admin area too but i wanna use this class in 
     both admin and deafult area thats why i create over here in default */

    /* this class is exaclt same as PAGEDTO, PAGEDTo use default values from table and 
     this class helps to extend their features like we add string required etc that 
     why we have two same class but with some differend properties*/
    public class PageVM
    {

        //type ctor and hit tab for shortcut
        public PageVM()
        {

        }


        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSlideBar = row.HasSlideBar;
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSlideBar { get; set; }
    }
}
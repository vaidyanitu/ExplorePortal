using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ExploreModel
{
   public class Site
    {
       public Site()
       {
           DateCreated = System.DateTime.Now;
       }
        public int SiteId { get; set; }
       [Required]
       [Display(Name="Site")]
        public string SiteName { get; set; }
       [Required]
       [Display(Name="Location")]
        public String  SiteLocation { get; set; }
       [AllowHtml]
       [Display(Name="Description")]
        public String SiteDescription { get; set; }
       public string Photo { get; set; }
       public List<Tag> Tags { get; set; }
       public int test { get; set; }
       public DateTime DateCreated { get; set; }
       public string AuthorId { get; set; }
                 }
}

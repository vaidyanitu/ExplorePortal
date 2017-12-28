using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploreModel
{
   public class SiteTagModel
    {
       [Key]
      public int SiteTagId { get; set; }
       public int SiteId { get; set; }
       public int TagId { get; set; }
    }
}

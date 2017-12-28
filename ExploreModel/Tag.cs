using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploreModel
{
   public class Tag
    {
       public Tag()
       {
           DateCreated = System.DateTime.Now;
       }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExploreModel
{
   public class ArticleTags
    {
       [Key]
        public int ArttagID { get; set; }
        public int ArticleId { get; set; }
        public int TagId { get; set; }
    }
}

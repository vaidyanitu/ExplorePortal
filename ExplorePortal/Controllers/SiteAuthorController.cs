using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using ExploreModel;

namespace ExplorePortal.Controllers
{
    public class SiteAuthorController : Controller
    {
        ExploreDbContext db = new ExploreDbContext(); 
        //
        // GET: /SiteAuthor/
        [Authorize]
        public ActionResult Index(int? page, string searchstring)
        {
            ViewBag.Tags = db.Tags.ToList();
            string userId = User.Identity.GetUserId();
            ViewBag.userId = userId;
            ViewBag.searchstring = searchstring;
            var Modellist = new List<Site>();
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var model = db.Site.Where(x => x.AuthorId == userId);
            if (!string.IsNullOrEmpty(searchstring))
            {
                model = model.Where(x => x.SiteLocation.Contains(searchstring) || x.SiteName.Contains(searchstring));
            }
            Modellist = model.OrderBy(x => x.SiteName).ToList();
            return View(Modellist.ToPagedList(pageNumber, pageSize));
        }                                  
       
	}
}
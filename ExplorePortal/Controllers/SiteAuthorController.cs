using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace ExplorePortal.Controllers
{
    public class SiteAuthorController : Controller
    {
        ExploreDbContext db = new ExploreDbContext(); 
        //
        // GET: /SiteAuthor/
        [Authorize]
        public ActionResult Index(int? page)
        {
            ViewBag.Tags = db.Tags.ToList();
            ViewBag.userId = User.Identity.GetUserId();
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                int pageNumber = (page ?? 1);
                int pageSize = 4;
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var userIdValue = userIdClaim.Value;
                    var model=db.Site.Where(x => x.AuthorId == userIdValue);
                    model=model.OrderBy(s => s.SiteName);
                    return View(model.ToPagedList(pageNumber,pageSize));
                }
            }
            return View();                           
        }
	}
}
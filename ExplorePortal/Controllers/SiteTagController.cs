using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExploreModel;
using ExplorePortal;
using PagedList;
using PagedList.Mvc;

namespace ExplorePortal.Controllers
{
   
    public class SiteTagController : Controller
    {
        
        private ExploreDbContext db = new ExploreDbContext();

        // GET: /SiteTag/
        public ActionResult Index()
        {
            return View(db.SiteTagModel.ToList());
        }

         [Authorize]
        // GET: /SiteTag/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteTagModel sitetagmodel = db.SiteTagModel.Find(id);
            if (sitetagmodel == null)
            {
                return HttpNotFound();
            }
            return View(sitetagmodel);
        }

         [Authorize]
        // GET: /SiteTag/Create
        public ActionResult Create()
        {
            return View();
        }

         [Authorize]
        // POST: /SiteTag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SiteTagId,SiteId,TagId")] SiteTagModel sitetagmodel)
        {
            if (ModelState.IsValid)
            {
                db.SiteTagModel.Add(sitetagmodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sitetagmodel);
        }

         [Authorize]
        // GET: /SiteTag/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteTagModel sitetagmodel = db.SiteTagModel.Find(id);
            if (sitetagmodel == null)
            {
                return HttpNotFound();
            }
            return View(sitetagmodel);
        }

         [Authorize]
        // POST: /SiteTag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SiteTagId,SiteId,TagId")] SiteTagModel sitetagmodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sitetagmodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sitetagmodel);
        }


         [Authorize]
        // GET: /SiteTag/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteTagModel sitetagmodel = db.SiteTagModel.Find(id);
            if (sitetagmodel == null)
            {
                return HttpNotFound();
            }
            return View(sitetagmodel);
        }

         [Authorize]
        // POST: /SiteTag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteTagModel sitetagmodel = db.SiteTagModel.Find(id);
            db.SiteTagModel.Remove(sitetagmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult GetSiteByTag(string sortOrder, string currentFilter, string searchString, string tagname, int? page)
        {
            var Hastag = tagname ?? "";
            ViewBag.TagName = tagname;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Tags = db.Tags.ToList();
            //Add viewbag to save sortorder of table
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "site_desc" : "";
            ViewBag.LocSortParm = string.IsNullOrEmpty(sortOrder) ? "loc_desc" : "";
            var ModelList = new List<Site>();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            using (var context = new ExploreDbContext())
            {
                var model = from s in context.Site select s;

                if (!string.IsNullOrEmpty(Hastag))
                {
                    var tagid = db.Tags.Where(x => x.TagName == Hastag).Select(x => x.TagId).First();
                    List<int> sitewithtag = db.SiteTagModel.Where(x => x.TagId == tagid).Select(x => x.SiteId).ToList();
                    model = model.Where(t => sitewithtag.Contains(t.SiteId));
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.SiteName.Contains(searchString) ||
                        s.SiteLocation.Contains(searchString));
                }

                //Switch action according to sortOrder
                switch (sortOrder)
                {
                    case "site_desc":
                        ModelList = model.OrderByDescending(s => s.SiteName).ToList();
                        break;
                    case "loc_desc":
                        ModelList = model.OrderByDescending(s => s.SiteLocation).ToList();
                        break;
                    default:
                        ModelList = model.OrderBy(s => s.SiteName).ToList();
                        break;
                }

                int pageNumber = (page ?? 1);
                int pageSize = 4;
                ModelList = model.ToList();
                //return View(ModelList.ToPagedList(pageNumber, pageSize));
                return PartialView("GetSiteByTagPartial", ModelList.ToPagedList(pageNumber, pageSize));
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

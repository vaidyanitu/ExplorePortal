using ExploreModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace ExplorePortal.Controllers
{
    public class SitesController : Controller
    {
        private ExploreDbContext db = new ExploreDbContext();

        //
        // GET: /Sites/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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
                return View(ModelList.ToPagedList(pageNumber, pageSize));
            }

        }

        [Authorize]
        public ActionResult AddSite()
        {
            ViewBag.Tags = db.Tags.ToList();
            return View();
        }

        //[Display(Name="Add")]
        //[HttpPost]
        //public ActionResult AddSite([Bind(Exclude="SiteId")]Site site, HttpPostedFileBase file)
        //{
        //    byte[] picarray=null;
        //    if (file != null)
        //    {
        //        string pic = System.IO.Path.GetFileName(file.FileName);
        //        string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);
        //        //file is uploaded
        //        file.SaveAs(path);
        //        // save the image path path to the database or you can send image
        //        // directly to database
        //        // in-case if you want to store byte[] ie. for DB
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            file.InputStream.CopyTo(ms);
        //            picarray = ms.GetBuffer();
        //        }
        //    }
        //    if(ModelState.IsValid)
        //    {
        //        site.Photo = picarray;
        //        db.Site.Add(site);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(site);
        //}

        [Authorize]
        [Display(Name = "Edit")]
        //Get:Explore/EditSite/5
        public ActionResult EditSite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Site.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        [Authorize]
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSite([Bind(Include = "SiteId,SiteName,SiteLocation,SiteDescription,Photo")] Site site, HttpPostedFileBase file)
        {
            byte[] picarray = null;
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);
                //file is uploaded
                file.SaveAs(path);
                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    picarray = ms.GetBuffer();
                }
            }
            if (ModelState.IsValid)
            {
                site.Photo = picarray;
                db.Entry(site).State = EntityState.Modified;
                if (file == null)
                {
                    db.Entry(site).Property(m => m.Photo).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        [Authorize]
        //get
        public ActionResult DeleteSite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Site.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);

        }

        [Authorize]
        [HttpPost, ActionName("DeleteSite")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Site site = db.Site.Find(id);
            db.Site.Remove(site);
            var query = from c in db.SiteTagModel
                        where c.SiteId == id
                        select c;
            foreach (var item in query)
            {
                db.SiteTagModel.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DetailsSite(int? id)
        {
            ViewBag.Tags = db.Tags.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Site.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddModelTag()
        {
            byte[] picarray = null;
            //  Get all files from Request object  
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;

                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                // Get the complete folder path and store the file inside it.  
                fname = Path.Combine(Server.MapPath("~/images"), fname);
                file.SaveAs(fname);

                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    picarray = ms.GetBuffer();
                }

            }

            //,SiteName,SiteLocation,SiteDescription
            var obj4 = Request.Form[3];
            string[] words = obj4.Split(',');
            string text = "";
            int[] tgid = new int[words.Length];
            int k = 0;
            foreach (var item in words)
            {
                var query = (from x in db.Tags
                             where x.TagName.Contains(item)
                             select x.TagId).First();

                text += item + ',';
                tgid[k] = query;
                k++;
            }

            Site site = new Site();
            site.SiteName = Request.Form[0];
            site.SiteLocation = Request.Form[2];
            site.SiteDescription = System.Uri.UnescapeDataString( Request.Form[1]);
            site.Photo = picarray;
            SiteTagModel stm = new SiteTagModel();
            if (ModelState.IsValid)
            {
                db.Site.Add(site);
                db.SaveChanges();
                foreach (var item in tgid)
                {
                    stm.SiteId = site.SiteId;
                    stm.TagId = item;
                    db.SiteTagModel.Add(stm);
                    db.SaveChanges();
                }

                return Json(new { redirecturl = "/Explore" }, JsonRequestBehavior.AllowGet);
            }
            return View(site);
        }
    }
}

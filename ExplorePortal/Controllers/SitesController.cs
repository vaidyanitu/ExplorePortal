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
using System.Security.Claims;
using Microsoft.AspNet.Identity;


namespace ExplorePortal.Controllers
{
    public class SitesController : Controller
    {
        private ExploreDbContext db = new ExploreDbContext();
        
        //
        // GET: /Sites/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string tagname)
        {
            //ViewBag.CurrentSort = sortOrder;
            ViewBag.Tags = db.Tags.ToList();
            //ViewBag.userId=User.Identity.GetUserId();
            ViewBag.tagname = tagname == null ? "" : tagname;

         
          
            return View();
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
            var thisuserId = User.Identity.GetUserId();
            var siteAuthorId = db.Site.Where(x=>x.SiteId==id).Select(m=>m.AuthorId).FirstOrDefault();
            if (thisuserId==siteAuthorId){
                ViewBag.userId = siteAuthorId.ToString();
            }
            else
            {
                ViewBag.userId = 0;
            }
            return View(site);
        }

        [Authorize]
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSite([Bind(Include = "SiteId,SiteName,SiteLocation,SiteDescription,Photo")] Site site, HttpPostedFileBase file)
        {
            var thisuserId =  User.Identity.GetUserId();
           var siteAuthorId = db.Site.Where(m => m.SiteId == site.SiteId).Select(x => x.AuthorId).FirstOrDefault();
           if (thisuserId == siteAuthorId)
           {
               if (Request.Files.Count > 0)
               {
                    file = Request.Files[0];

                   if (file != null && file.ContentLength > 0)
                   {
                       var fileName = Path.GetFileName(file.FileName);
                       var path = Path.Combine(Server.MapPath("~/Resources/"), fileName);
                       file.SaveAs(path);
                       site.Photo = path;
                   }
               }
               if (ModelState.IsValid)
               {
                   db.Entry(site).State = EntityState.Modified;
                   if (file == null)
                   {
                       db.Entry(site).Property(m => m.Photo).IsModified = false;                     
                   }
                   db.Entry(site).Property(m => m.AuthorId).IsModified = false;
                   db.SaveChanges();
                   ViewBag.userId = siteAuthorId;
                   return RedirectToAction("Index");
               }
           }
           else
           {
               ViewBag.userId = "";
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
            var thisuserId = User.Identity.GetUserId();
            ViewBag.UserId = thisuserId;
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
            Site site = new Site();

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Resources/"), fileName);
         
                    file.SaveAs(path);
             
                    site.Photo = path;
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

           
            site.SiteName = Request.Form[0];
            site.SiteLocation = Request.Form[2];
            site.SiteDescription = System.Uri.UnescapeDataString( Request.Form[1]);
            var thisuserId = User.Identity.GetUserId();
          
            if (thisuserId!=null){
                site.AuthorId = thisuserId;
            }
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


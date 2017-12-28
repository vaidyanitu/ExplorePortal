using ExploreModel;
using ExplorePortal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ExplorePortal
{
    public class ExploreDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExploreDbContext():base("DefaultConnection")
        {

        }
               public DbSet<Site> Site { get; set; }

               public System.Data.Entity.DbSet<ExploreModel.Tag> Tags { get; set; }

               public DbSet<SiteTagModel> SiteTagModel { get; set; }
    }
}
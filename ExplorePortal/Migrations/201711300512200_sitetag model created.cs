namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sitetagmodelcreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteTagModels",
                c => new
                    {
                        SiteTagId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SiteTagId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SiteTagModels");
        }
    }
}

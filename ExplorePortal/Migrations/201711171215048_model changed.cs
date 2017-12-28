namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                        Site_SiteId = c.Int(),
                    })
                .PrimaryKey(t => t.TagId)
                .ForeignKey("dbo.Sites", t => t.Site_SiteId)
                .Index(t => t.Site_SiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "Site_SiteId", "dbo.Sites");
            DropIndex("dbo.Tags", new[] { "Site_SiteId" });
            DropTable("dbo.Tags");
        }
    }
}

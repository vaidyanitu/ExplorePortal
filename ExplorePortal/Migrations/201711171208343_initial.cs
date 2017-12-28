namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(nullable: false),
                        SiteLocation = c.String(),
                        SiteDescription = c.String(),
                        Photo = c.Binary(storeType: "image"),
                        test = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SiteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sites");
        }
    }
}

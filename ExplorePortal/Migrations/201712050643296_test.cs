namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sites", "SiteLocation", c => c.String(nullable: false));
            AlterColumn("dbo.Sites", "SiteDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sites", "SiteDescription", c => c.String());
            AlterColumn("dbo.Sites", "SiteLocation", c => c.String());
        }
    }
}

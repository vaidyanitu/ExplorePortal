namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class authoridforsitesmodified : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sites", "AuthorId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sites", "AuthorId", c => c.Int(nullable: false));
        }
    }
}

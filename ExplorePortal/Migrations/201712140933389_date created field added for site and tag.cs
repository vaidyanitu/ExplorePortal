namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datecreatedfieldaddedforsiteandtag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tags", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "DateCreated");
            DropColumn("dbo.Sites", "DateCreated");
        }
    }
}

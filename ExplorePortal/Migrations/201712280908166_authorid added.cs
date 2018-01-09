namespace ExplorePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class authoridadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sites", "AuthorId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sites", "AuthorId");
        }
    }
}

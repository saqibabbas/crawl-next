namespace crawl_next.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pages_changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "page", c => c.Int(nullable: false));
            DropColumn("dbo.MainDocuments", "page");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MainDocuments", "page", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "page");
        }
    }
}

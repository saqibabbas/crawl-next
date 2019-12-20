namespace crawl_next.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimaryDocument : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        url = c.String(),
                        page = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "mainDocumentId", c => c.Int());
            CreateIndex("dbo.Documents", "mainDocumentId");
            AddForeignKey("dbo.Documents", "mainDocumentId", "dbo.MainDocuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "mainDocumentId", "dbo.MainDocuments");
            DropIndex("dbo.Documents", new[] { "mainDocumentId" });
            DropColumn("dbo.Documents", "mainDocumentId");
            DropTable("dbo.MainDocuments");
        }
    }
}

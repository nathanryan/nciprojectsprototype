namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTech : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Submissions", "StudentTechnologiesID", "dbo.StudentTechnologies");
            DropForeignKey("dbo.StudentTechnologies", "TechnologiesID", "dbo.Technologies");
            DropIndex("dbo.Submissions", new[] { "StudentTechnologiesID" });
            DropIndex("dbo.StudentTechnologies", new[] { "TechnologiesID" });
            DropColumn("dbo.Submissions", "StudentTechnologiesID");
            DropTable("dbo.StudentTechnologies");
            DropTable("dbo.Technologies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Technologies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        tech_name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StudentTechnologies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TechnologiesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Submissions", "StudentTechnologiesID", c => c.Int(nullable: false));
            CreateIndex("dbo.StudentTechnologies", "TechnologiesID");
            CreateIndex("dbo.Submissions", "StudentTechnologiesID");
            AddForeignKey("dbo.StudentTechnologies", "TechnologiesID", "dbo.Technologies", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Submissions", "StudentTechnologiesID", "dbo.StudentTechnologies", "ID", cascadeDelete: true);
        }
    }
}

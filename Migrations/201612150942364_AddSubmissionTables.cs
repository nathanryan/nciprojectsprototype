namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubmissionTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Submissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        linkedin_url = c.String(),
                        project_title = c.String(),
                        short_desc = c.String(),
                        long_desc = c.String(),
                        StudentTechnologiesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.StudentTechnologies", t => t.StudentTechnologiesID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.StudentTechnologiesID);
            
            CreateTable(
                "dbo.StudentTechnologies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TechnologiesID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Submissions", "StudentTechnologiesID", "dbo.StudentTechnologies");
            DropForeignKey("dbo.Submissions", "StudentID", "dbo.Students");
            DropIndex("dbo.Submissions", new[] { "StudentTechnologiesID" });
            DropIndex("dbo.Submissions", new[] { "StudentID" });
            DropTable("dbo.StudentTechnologies");
            DropTable("dbo.Submissions");
        }
    }
}

namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Student_StudentID = c.Int(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Students", t => t.Student_StudentID)
                .Index(t => t.Student_StudentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Student_StudentID", "dbo.Students");
            DropIndex("dbo.Files", new[] { "Student_StudentID" });
            DropTable("dbo.Files");
        }
    }
}

namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFileField : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Student_StudentID", "dbo.Students");
            DropIndex("dbo.Files", new[] { "Student_StudentID" });
            RenameColumn(table: "dbo.Files", name: "Student_StudentID", newName: "StudentID");
            AlterColumn("dbo.Files", "StudentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "StudentID");
            AddForeignKey("dbo.Files", "StudentID", "dbo.Students", "StudentID", cascadeDelete: true);
            DropColumn("dbo.Files", "PersonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "PersonId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Files", "StudentID", "dbo.Students");
            DropIndex("dbo.Files", new[] { "StudentID" });
            AlterColumn("dbo.Files", "StudentID", c => c.Int());
            RenameColumn(table: "dbo.Files", name: "StudentID", newName: "Student_StudentID");
            CreateIndex("dbo.Files", "Student_StudentID");
            AddForeignKey("dbo.Files", "Student_StudentID", "dbo.Students", "StudentID");
        }
    }
}

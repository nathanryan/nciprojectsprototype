namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStudentTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        StudentNumber = c.Int(nullable: false),
                        fname = c.String(),
                        lname = c.String(),
                        CourseID = c.Int(nullable: false),
                        StreamID = c.Int(nullable: false),
                        SupervisorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StudentID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Streams", t => t.StreamID, cascadeDelete: true)
                .ForeignKey("dbo.Supervisors", t => t.SupervisorID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.StreamID)
                .Index(t => t.SupervisorID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        course_name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        stream_name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Supervisors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        first_name = c.String(),
                        last_name = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.UserAccount", "StudentNumber", c => c.Int());
            AddColumn("dbo.UserAccount", "Students_StudentID", c => c.Int());
            CreateIndex("dbo.UserAccount", "Students_StudentID");
            AddForeignKey("dbo.UserAccount", "Students_StudentID", "dbo.Students", "StudentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccount", "Students_StudentID", "dbo.Students");
            DropForeignKey("dbo.Students", "SupervisorID", "dbo.Supervisors");
            DropForeignKey("dbo.Students", "StreamID", "dbo.Streams");
            DropForeignKey("dbo.Students", "CourseID", "dbo.Courses");
            DropIndex("dbo.UserAccount", new[] { "Students_StudentID" });
            DropIndex("dbo.Students", new[] { "SupervisorID" });
            DropIndex("dbo.Students", new[] { "StreamID" });
            DropIndex("dbo.Students", new[] { "CourseID" });
            DropColumn("dbo.UserAccount", "Students_StudentID");
            DropColumn("dbo.UserAccount", "StudentNumber");
            DropTable("dbo.Supervisors");
            DropTable("dbo.Streams");
            DropTable("dbo.Courses");
            DropTable("dbo.Students");
        }
    }
}

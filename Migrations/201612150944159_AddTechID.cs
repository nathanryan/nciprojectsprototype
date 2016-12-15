namespace NCIProjects.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTechID : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Technologies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        tech_name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.StudentTechnologies", "TechnologiesID");
            AddForeignKey("dbo.StudentTechnologies", "TechnologiesID", "dbo.Technologies", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTechnologies", "TechnologiesID", "dbo.Technologies");
            DropIndex("dbo.StudentTechnologies", new[] { "TechnologiesID" });
            DropTable("dbo.Technologies");
        }
    }
}

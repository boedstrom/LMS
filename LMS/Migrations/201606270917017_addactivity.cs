namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addactivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddActivityViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        ModuleName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AddActivityViewModels");
        }
    }
}

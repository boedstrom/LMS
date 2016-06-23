namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModules2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AddModuleViewModels", "Name");
            DropColumn("dbo.AddModuleViewModels", "Description");
            DropColumn("dbo.AddModuleViewModels", "StartDate");
            DropColumn("dbo.AddModuleViewModels", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AddModuleViewModels", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AddModuleViewModels", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AddModuleViewModels", "Description", c => c.String());
            AddColumn("dbo.AddModuleViewModels", "Name", c => c.String());
        }
    }
}

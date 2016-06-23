namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zz1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AddModuleViewModels", "CourseName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AddModuleViewModels", "CourseName");
        }
    }
}

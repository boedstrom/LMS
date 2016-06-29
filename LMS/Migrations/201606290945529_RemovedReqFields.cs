namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedReqFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Name", c => c.String());
            AlterColumn("dbo.Documents", "Name", c => c.String());
            AlterColumn("dbo.Courses", "Name", c => c.String());
            AlterColumn("dbo.Modules", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Documents", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false));
        }
    }
}

namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2nd_160628 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserViewModels", "Course_Id", c => c.Int());
            CreateIndex("dbo.UserViewModels", "Course_Id");
            AddForeignKey("dbo.UserViewModels", "Course_Id", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserViewModels", "Course_Id", "dbo.Courses");
            DropIndex("dbo.UserViewModels", new[] { "Course_Id" });
            DropColumn("dbo.UserViewModels", "Course_Id");
        }
    }
}

namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pets", "PetName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pets", "PetName", c => c.String());
        }
    }
}

namespace ANPositive.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AteamComTrDb3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contents", "MenuPosition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contents", "MenuPosition");
        }
    }
}

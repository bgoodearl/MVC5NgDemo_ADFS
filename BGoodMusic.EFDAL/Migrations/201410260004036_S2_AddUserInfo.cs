namespace BGoodMusic.EFDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class S2_AddUserInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.bgm_UserInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        UserIdentifier = c.String(nullable: false, maxLength: 384),
                        Token = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.UserIdentifier, t.Id }, name: "IX_UserIdId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.bgm_UserInfo", "IX_UserIdId");
            DropTable("dbo.bgm_UserInfo");
        }
    }
}

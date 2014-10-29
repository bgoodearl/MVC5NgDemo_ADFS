namespace BGoodMusic.EFDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Schema1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.bgm_Rehearsals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                        Duration = c.Time(precision: 7),
                        Location = c.String(maxLength: 512),
                        Agenda = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.bgm_Rehearsals");
        }
    }
}

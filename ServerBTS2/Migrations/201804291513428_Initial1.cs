namespace ServerBTS2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HinhAnhs",
                c => new
                    {
                        IDHinhAnh = c.Int(nullable: false, identity: true),
                        IDTheLoaiAnh = c.Int(nullable: false),
                        IDChung = c.Int(nullable: false),
                        TenAnh = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDHinhAnh);
            
            CreateTable(
                "dbo.TheLoaiAnhs",
                c => new
                    {
                        IDTheLoaiAnh = c.Int(nullable: false, identity: true),
                        TenTheLoai = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDTheLoaiAnh);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TheLoaiAnhs");
            DropTable("dbo.HinhAnhs");
        }
    }
}

namespace Recargas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Usuario = c.String(nullable: false, maxLength: 128),
                        Senha = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Usuario);
            
            CreateTable(
                "dbo.Compra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PedidoProducto",
                c => new
                    {
                        PedidoId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PedidoId, t.ProductoId })
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.PedidoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Pedido",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha_Pedido = c.DateTime(nullable: false),
                        Fecha_Modificacion = c.DateTime(nullable: false),
                        PuntoId = c.Int(nullable: false),
                        PersonaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persona", t => t.PersonaId, cascadeDelete: true)
                .ForeignKey("dbo.Punto", t => t.PuntoId)
                .Index(t => t.PuntoId)
                .Index(t => t.PersonaId);
            
            CreateTable(
                "dbo.Pagamento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PedidoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pedido", t => t.PedidoId)
                .Index(t => t.PedidoId);
            
            CreateTable(
                "dbo.Persona",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Telefono = c.String(),
                        Observaciones = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Punto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Direccion = c.String(),
                        RutaId = c.Int(nullable: false),
                        ProprietarioId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Persona", t => t.ProprietarioId)
                .ForeignKey("dbo.Ruta", t => t.RutaId)
                .Index(t => t.RutaId)
                .Index(t => t.ProprietarioId);
            
            CreateTable(
                "dbo.Ruta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Compra", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.PedidoProducto", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Pedido", "PuntoId", "dbo.Punto");
            DropForeignKey("dbo.Pedido", "PersonaId", "dbo.Persona");
            DropForeignKey("dbo.Punto", "RutaId", "dbo.Ruta");
            DropForeignKey("dbo.Punto", "ProprietarioId", "dbo.Persona");
            DropForeignKey("dbo.PedidoProducto", "PedidoId", "dbo.Pedido");
            DropForeignKey("dbo.Pagamento", "PedidoId", "dbo.Pedido");
            DropIndex("dbo.Punto", new[] { "ProprietarioId" });
            DropIndex("dbo.Punto", new[] { "RutaId" });
            DropIndex("dbo.Pagamento", new[] { "PedidoId" });
            DropIndex("dbo.Pedido", new[] { "PersonaId" });
            DropIndex("dbo.Pedido", new[] { "PuntoId" });
            DropIndex("dbo.PedidoProducto", new[] { "ProductoId" });
            DropIndex("dbo.PedidoProducto", new[] { "PedidoId" });
            DropIndex("dbo.Compra", new[] { "ProductoId" });
            DropTable("dbo.Ruta");
            DropTable("dbo.Punto");
            DropTable("dbo.Persona");
            DropTable("dbo.Pagamento");
            DropTable("dbo.Pedido");
            DropTable("dbo.PedidoProducto");
            DropTable("dbo.Producto");
            DropTable("dbo.Compra");
            DropTable("dbo.Account");
        }
    }
}

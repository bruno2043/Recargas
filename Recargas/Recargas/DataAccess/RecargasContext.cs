using Recargas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Recargas.DataAccess
{
    public class RecargasContext : DbContext
    {
        public DbSet<Ruta> Ruta { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Punto> Punto { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }
        public DbSet<PedidoProducto> PedidoProducto { get; set; }
        public DbSet<Account> Account { get; set; }
        public RecargasContext() : base("name=RecargasDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RecargasContext, Recargas.Migrations.Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Punto>()
            .HasRequired<Persona>(s => s.Proprietario)
            .WithMany(g => g.Puntos)
            .HasForeignKey<int>(s => s.ProprietarioId);

            modelBuilder.Entity<Punto>()
            .HasRequired<Ruta>(s => s.Ruta)
            .WithMany(g => g.Puntos)
            .HasForeignKey<int>(s => s.RutaId);

            modelBuilder.Entity<Compra>()
            .HasRequired<Producto>(s => s.Producto)
            .WithMany(g => g.Compras)
            .HasForeignKey<int>(s => s.ProductoId);

            modelBuilder.Entity<Pedido>()
            .HasRequired<Punto>(s => s.Punto)
            .WithMany(g => g.Pedidos)
            .HasForeignKey<int>(s => s.PuntoId);

            modelBuilder.Entity<Pedido>()
            .HasRequired<Persona>(s => s.Persona)
            .WithMany(g => g.Pedidos)
            .HasForeignKey<int>(s => s.PersonaId)
            .WillCascadeOnDelete(true); ;

            modelBuilder.Entity<Pagamento>()
            .HasRequired<Pedido>(s => s.Pedido)
            .WithMany(g => g.Pagamentos)
            .HasForeignKey<int>(s => s.PedidoId);

            //modelBuilder.Entity<PedidoProducto>()
            //.HasRequired<Pedido>(s => s.Pedido)
            //.WithMany(g => g.ItensPedido)
            //.HasForeignKey<int>(s => s.PedidoId);

            //modelBuilder.Entity<PedidoProducto>()
            //.HasRequired<Producto>(s => s.Producto)
            //.WithMany(g => g.ItensPedido)
            //.HasForeignKey<int>(s => s.ProductoId);

            modelBuilder.Entity<PedidoProducto>()
            .HasKey(s => new { s.PedidoId, s.ProductoId });
        }

    }
}
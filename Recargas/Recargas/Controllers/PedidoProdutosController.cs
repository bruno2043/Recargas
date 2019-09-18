using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Recargas.DataAccess;
using Recargas.Models;

namespace Recargas.Controllers
{
    public class PedidoProductosController : Controller
    {
        private RecargasContext db = new RecargasContext();

        // GET: PedidoProductos
        public ActionResult Index()
        {
            var PedidoProducto = db.PedidoProducto.Include(p => p.Pedido).Include(p => p.Producto);
            return View(PedidoProducto.ToList());
        }

        // GET: PedidoProductos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoProducto PedidoProducto = db.PedidoProducto.Find(id);
            if (PedidoProducto == null)
            {
                return HttpNotFound();
            }
            return View(PedidoProducto);
        }

        // GET: PedidoProductos/Create
        public ActionResult Create()
        {
            ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id");
            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome");
            return View();
        }

        // POST: PedidoProductos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PedidoId,ProductoId,Fecha_Modificacion,Cantidad,Precio,Status")] PedidoProducto PedidoProducto)
        {
            if (ModelState.IsValid)
            {
                db.PedidoProducto.Add(PedidoProducto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id", PedidoProducto.PedidoId);
            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome", PedidoProducto.ProductoId);
            return View(PedidoProducto);
        }

        // GET: PedidoProductos/Edit/5
        public ActionResult Edit(int? idPedido, int? idProducto)
        {
            if (idPedido == null || idProducto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoProducto pedidoProducto = db.PedidoProducto.Where(pp => pp.PedidoId == (int)idPedido
                && pp.ProductoId == (int)idProducto).FirstOrDefault();
            if (pedidoProducto == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome", String.Empty);

            return View(pedidoProducto);
        }

        // POST: PedidoProductos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PedidoProducto PedidoProducto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(PedidoProducto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Pedidos", new { @id = PedidoProducto.PedidoId }); 
            }
            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome", PedidoProducto.ProductoId);
            return View(PedidoProducto);
        }

        // GET: PedidoProductos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoProducto PedidoProducto = db.PedidoProducto.Find(id);
            if (PedidoProducto == null)
            {
                return HttpNotFound();
            }
            return View(PedidoProducto);
        }

        // POST: PedidoProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PedidoProducto PedidoProducto = db.PedidoProducto.Find(id);
            db.PedidoProducto.Remove(PedidoProducto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

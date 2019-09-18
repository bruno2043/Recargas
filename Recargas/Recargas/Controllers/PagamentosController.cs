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
    public class PagamentosController : Controller
    {
        private RecargasContext db = new RecargasContext();

        // GET: Pagamentos
        public ActionResult Index()
        {
            var pagamento = db.Pagamento.Include(p => p.Pedido);
            return View(pagamento.ToList());
        }

        // GET: Pagamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagamento pagamento = db.Pagamento.Find(id);
            if (pagamento == null)
            {
                return HttpNotFound();
            }
            return View(pagamento);
        }

        // GET: Pagamentos/Create
        public ActionResult Create(int? idPedido)
        {
            if (idPedido == null)
            {
                return HttpNotFound();
            }
            Pagamento pagamento = new Pagamento();
            pagamento.Fecha = DateTime.Now;
            pagamento.PedidoId = (int)idPedido;
            //ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id");
            return View(pagamento);
        }

        // POST: Pagamentos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha,Valor,PedidoId")] Pagamento pagamento)
        {
            if (ModelState.IsValid)
            {
                decimal entregue = db.PedidoProducto.Where(pp => pp.PedidoId == pagamento.PedidoId && pp.Status == Status.Entregue)
                    .Select(pp => (pp.Cantidad * pp.Precio))
                    .DefaultIfEmpty(0)
                    .Sum();
                decimal pago = db.Pagamento.Where(p => p.PedidoId == pagamento.PedidoId)
                    .Select(p => p.Valor)
                    .DefaultIfEmpty(0)
                    .Sum();
                if (entregue - pago - pagamento.Valor >= 0)
                {
                    db.Pagamento.Add(pagamento);
                    db.SaveChanges();
                }
                return RedirectToAction("Edit", "Pedidos", new { @id = pagamento.PedidoId });
            }

            ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id", pagamento.PedidoId);
            return View(pagamento);
        }

        // GET: Pagamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagamento pagamento = db.Pagamento.Find(id);
            if (pagamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id", pagamento.PedidoId);
            return View(pagamento);
        }

        // POST: Pagamentos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Fecha,Valor,PedidoId")] Pagamento pagamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pagamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PedidoId = new SelectList(db.Pedido, "Id", "Id", pagamento.PedidoId);
            return View(pagamento);
        }

        // GET: Pagamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagamento pagamento = db.Pagamento.Find(id);
            if (pagamento == null)
            {
                return HttpNotFound();
            }
            return View(pagamento);
        }

        // POST: Pagamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pagamento pagamento = db.Pagamento.Find(id);
            db.Pagamento.Remove(pagamento);
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

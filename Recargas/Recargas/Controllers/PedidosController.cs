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
    [Authorize(Roles = "admin")]
    public class PedidosController : Controller
    {
        private RecargasContext db = new RecargasContext();

        // GET: Pedidos
        public ActionResult Index(string FilterStatus)
        {
            ViewBag.ListaStatus = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = v.ToString()
            }).ToList(), "Value", "Text");
            
            if (FilterStatus == null)
            {
                FilterStatus = Status.Pedido.ToString();                    
            }
            
            var pedidoF = db.Pedido
                .Where(p =>p.PedidoProductos.Where(pp => pp.Status.ToString() == FilterStatus)
                            .Select(i => i.PedidoId).Contains(p.Id))
                .OrderBy(x => x.Punto.Ruta.Nome).ThenBy(x=>x.Fecha_Pedido)
                .Include(p => p.Persona).Include(p => p.Punto).Include(p=>p.Punto.Ruta);
            return View(pedidoF.ToList());


        }

        // GET: Pedidos
        public ActionResult IndexDeuda()
        {
            var pedido = db.Pedido
                .Where(p => (p.PedidoProductos.Where(pp => pp.Status == Status.Entregue)
                            .Select(i => i.Cantidad * i.Precio).DefaultIfEmpty(0).Sum()) >
                            (p.Pagamentos
                            .Select(i => i.Valor).DefaultIfEmpty(0).Sum()))
                .OrderBy(x => x.Punto.Ruta.Nome).ThenBy(x => x.Fecha_Pedido)
                .Include(p => p.Persona).Include(p => p.Punto).Include(p => p.Punto.Ruta);
            return View(pedido.ToList());


        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.PersonaId = new SelectList(db.Persona, "Id", "Nome");
            ViewBag.PuntoId = new SelectList(db.Punto, "Id", "Nome");
            Pedido pedido = new Pedido();
            pedido.Fecha_Pedido = DateTime.Now;
            pedido.Fecha_Modificacion = DateTime.Now;
            pedido.PedidoProductos = new List<PedidoProducto>();
            return View(pedido);
        }

        // POST: Pedidos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Fecha_Pedido,Fecha_Modificacion,PuntoId,PersonaId")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("/Edit", pedido);
            }

            ViewBag.PersonaId = new SelectList(db.Persona, "Id", "Nome", pedido.PersonaId);
            ViewBag.PuntoId = new SelectList(db.Punto, "Id", "Nome", pedido.PuntoId);
            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return RedirectToAction("Index"); //HttpNotFound();
            }
            pedido.PedidoProducto = new PedidoProducto();
            pedido.PedidoProducto.Fecha_Modificacion = DateTime.Now;
            pedido.PedidoProducto.PedidoId = pedido.Id;
            pedido.PedidoProductos = db.PedidoProducto.Where(pp => pp.PedidoId == pedido.Id).ToList();
            if (pedido.PedidoProductos == null) pedido.PedidoProductos = new List<PedidoProducto>();
            pedido.Persona = db.Persona.Find(pedido.PersonaId);
            pedido.Punto = db.Punto.Find(pedido.PuntoId);
            pedido.TotalEntregue = db.PedidoProducto
                .Where(pp => pp.PedidoId == pedido.Id && pp.Status == Status.Entregue)
                .Select(pp => (pp.Cantidad * pp.Precio))
                .DefaultIfEmpty(0)
                .Sum();

            pedido.TotalPago = db.Pagamento
                .Where(p => p.PedidoId == pedido.Id)
                .Select(p => p.Valor)
                .DefaultIfEmpty(0)
                .Sum();

            pedido.TotalDeuda = pedido.TotalEntregue - pedido.TotalPago;

            pedido.Pagamentos = db.Pagamento.Where(p => p.PedidoId == id).ToList();

            ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome", String.Empty);
            //ViewBag.ListaStatus = new SelectList(Status, "Id", "Nome", String.Empty);

            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                if (pedido.PedidoProducto.Cantidad>0 && pedido.PedidoProducto.Precio > 0)
                {
                    db.Entry(pedido.PedidoProducto).State = EntityState.Added;
                    db.SaveChanges();
                }
                return RedirectToAction("Edit", pedido.Id);
            }
            //ViewBag.ProductoId = new SelectList(db.Producto, "Id", "Nome", pedido.PedidoProducto.ProductoId);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            if (db.PedidoProducto.Where(pp=>pp.PedidoId==id && pp.Status == Status.Entregue).Count() == 0)
            {
                foreach (var item in db.PedidoProducto.Where(pp => pp.PedidoId == id))
                {
                    db.PedidoProducto.Remove(item);
                }
                db.Pedido.Remove(pedido);
                db.SaveChanges();
            }
            
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

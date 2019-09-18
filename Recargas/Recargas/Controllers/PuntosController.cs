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
    public class PuntosController : Controller
    {
        private RecargasContext db = new RecargasContext();

        // GET: Puntos
        public ActionResult Index()
        {
            var punto = db.Punto.Include(p => p.Proprietario).Include(p => p.Ruta);
            return View(punto.ToList());
        }

        // GET: Puntos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punto punto = db.Punto.Find(id);
            if (punto == null)
            {
                return HttpNotFound();
            }
            return View(punto);
        }

        // GET: Puntos/Create
        public ActionResult Create()
        {
            ViewBag.ProprietarioId = new SelectList(db.Persona, "Id", "Nome");
            ViewBag.RutaId = new SelectList(db.Ruta, "Id", "Nome");
            return View();
        }

        // POST: Puntos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Direccion,RutaId,ProprietarioId")] Punto punto)
        {
            if (ModelState.IsValid)
            {
                db.Punto.Add(punto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProprietarioId = new SelectList(db.Persona, "Id", "Nome", punto.ProprietarioId);
            ViewBag.RutaId = new SelectList(db.Ruta, "Id", "Nome", punto.RutaId);
            return View(punto);
        }

        // GET: Puntos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punto punto = db.Punto.Find(id);
            if (punto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProprietarioId = new SelectList(db.Persona, "Id", "Nome", punto.ProprietarioId);
            ViewBag.RutaId = new SelectList(db.Ruta, "Id", "Nome", punto.RutaId);
            return View(punto);
        }

        // POST: Puntos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Direccion,RutaId,ProprietarioId")] Punto punto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(punto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProprietarioId = new SelectList(db.Persona, "Id", "Nome", punto.ProprietarioId);
            ViewBag.RutaId = new SelectList(db.Ruta, "Id", "Nome", punto.RutaId);
            return View(punto);
        }

        // GET: Puntos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punto punto = db.Punto.Find(id);
            if (punto == null)
            {
                return HttpNotFound();
            }
            return View(punto);
        }

        // POST: Puntos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Punto punto = db.Punto.Find(id);
            db.Punto.Remove(punto);
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

        public JsonResult GetPersona(int puntoId)
        {
            string persona = db.Punto.Where(p => p.Id == puntoId).FirstOrDefault().ProprietarioId.ToString();
            return Json(persona, JsonRequestBehavior.AllowGet);
        }
    }
}

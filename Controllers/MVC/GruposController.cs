﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaControle.Models;

namespace SistemaControle.Controllers.MVC
{
    [Authorize(Roles = "Professor")]  
    public class GruposController : Controller
    {
        private ControleContext db = new ControleContext();

        //EXCLUIR ALUNO DO GRUPO
        public ActionResult ExcluirEstudante(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gruposDetalhes = db.GruposDetalhes.Find(id);
            if (gruposDetalhes == null)
            {
                return HttpNotFound();
            }
            db.GruposDetalhes.Remove(gruposDetalhes);
            db.SaveChanges();
            return RedirectToAction(string.Format("Details/{0}", gruposDetalhes.GrupoId));
        }
        //POST PARA ADD ALUNOS
        [HttpPost]
        public ActionResult AddEstudante(GruposDetalhes gruposDetalhes)
        {

            if (ModelState.IsValid)
            {
                var existe = db.GruposDetalhes.Where(gd => gd.GrupoId == gruposDetalhes.GrupoId && gd.UserId == gruposDetalhes.UserId).FirstOrDefault();
                if (existe == null)
                {
                    db.GruposDetalhes.Add(gruposDetalhes);
                    db.SaveChanges();
                    return RedirectToAction(string.Format("Details/{0}", gruposDetalhes.GrupoId));
                }
            }
                ModelState.AddModelError(string.Empty, "Aluno já Matriculado");

                ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Estudante).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", gruposDetalhes.UserId);
                return View(gruposDetalhes);
            }
            
        
    
       
        public ActionResult AddEstudante(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
          

            var gruposDetalhes = new GruposDetalhes
            {
                GrupoId = id.Value,
            };

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Estudante).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");

            return View(gruposDetalhes);
        }

       
        

       
        public ActionResult Index()
        {
            var grupos = db.Grupos.Include(g => g.Professor);
            return View(grupos.ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

       
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View();
        }

       
        // obter mais detalhes, consulte http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GrupoId,Descricao,UserId")] Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Grupos.Add(grupos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto");
            return View(grupos);
        }

     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", grupos.GrupoId);
            return View(grupos);
        }

       
        // obter mais detalhes, consulte http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GrupoId,Descricao,UserId")] Grupos grupos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grupos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Usuarios.Where(u => u.Professor).OrderBy(u => u.Nome).ThenBy(u => u.Sobrenome), "UserId", "NomeCompleto", grupos.GrupoId);
            return View(grupos);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupos grupos = db.Grupos.Find(id);
            if (grupos == null)
            {
                return HttpNotFound();
            }
            return View(grupos);
        }

               [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grupos grupos = db.Grupos.Find(id);
            db.Grupos.Remove(grupos);
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

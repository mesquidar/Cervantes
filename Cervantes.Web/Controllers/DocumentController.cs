using Cervantes.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class DocumentController : Controller
    {
        IDocumentManager documentManager = null;
        public DocumentController(IDocumentManager documentManager)
        {
            this.documentManager = documentManager;
        }

        // GET: DocumentController
        public ActionResult Index()
        {
            try
            {

                var model = documentManager.GetAll().Select(e => new CORE.Document
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    FilePath = e.FilePath,
                    User = e.User,
                    UserId = e.UserId,

                });

                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    TempData["empty"] = "No clients introduced";
                    return View();
                }


            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: DocumentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DocumentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DocumentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DocumentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DocumentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DocumentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

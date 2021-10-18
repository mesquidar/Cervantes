using Cervantes.Contracts;
using Cervantes.CORE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class ClientController : Controller
    {
        IClientManager clientManager = null;

        /// <summary>
        /// Client Controller Constructor
        /// </summary>
        public ClientController(IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }


        // GET: ClientController
        public ActionResult Index()
        {
            try
            {
                
               var model = clientManager.GetAll().Select(e => new CORE.Client
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    ContactEmail = e.ContactEmail,
                    ContactName = e.ContactName,
                    ContactPhone = e.ContactPhone,
                    Url = e.Url,

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

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create(Client model)
        {
            try
            {

                Client client = new Client
                {
                    Name = model.Name,
                    Description = model.Description,
                    ContactPhone= model.ContactPhone,
                    ContactName= model.ContactName,
                    ContactEmail= model.ContactEmail,
                    Url = model.Url,
                };
                clientManager.Add(client);
                clientManager.Context.SaveChanges();
                TempData["created"] = "created";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //guardamo log si hay un excepcion
                return View();
            }
        }

        // GET: ClientController/Edit/5
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Edit(int id)
        {
            try
            {
                //obtenemos la categoria a editar mediante su id
                var result = clientManager.GetById(id);

                Client client = new Client
                {
                    Name = result.Name,
                    Description = result.Description,
                    ContactEmail = result.ContactEmail,
                    ContactName= result.ContactName,
                    ContactPhone= result.ContactPhone,
                    Url= result.Url,
                };

                return View(client);
            }
            catch (Exception ex)
            {
                //guardamos log si hay excepcion
                return View();

            }
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Edit(int id, Client model)
        {
            try
            {
                var result = clientManager.GetById(id);
                result.Name = model.Name;
                result.Description = model.Description;
                result.ContactEmail = model.ContactEmail;
                result.ContactName = model.ContactName;
                result.ContactPhone = model.ContactPhone;
                result.Url = model.Url;

                clientManager.Context.SaveChanges();
                TempData["edited"] = "edited";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //guardamos log si hay excepcion
                return View();
            }
        }

        // GET: ClientController/Delete/5
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Delete(int id)
        {
            try
            {
                var client = clientManager.GetById(id);
                if (client != null)
                {
                    Client model = new Client
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Description = client.Description,
                        ContactEmail = client.ContactEmail,
                        ContactName = client.ContactName,
                        ContactPhone = client.ContactPhone,
                        Url = client.Url,

                    };
                    return View(model);
                }
            }
            catch (Exception e)
            {
                // guarda log si ocurre excepcion
                Redirect("Error");
            }

            return View();
        }

        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var client = clientManager.GetById(id);
                if (client != null)
                {
                    clientManager.Remove(client);
                    clientManager.Context.SaveChanges();
                }

                TempData["deleted"] = "deleted";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}

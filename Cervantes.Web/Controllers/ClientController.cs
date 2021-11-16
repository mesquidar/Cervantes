﻿using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Cervantes.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IClientManager clientManager = null;
        IUserManager userManager = null;

        /// <summary>
        /// Client Controller Constructor
        /// </summary>
        public ClientController(IClientManager clientManager, IUserManager userManager, IHostingEnvironment _appEnvironment)
        {
            this.clientManager = clientManager;
            this.userManager = userManager;
            this._appEnvironment = _appEnvironment;
        }


        /// <summary>
        /// Method Index shows all clients
        /// </summary>
        /// <returns>All Clients</returns>
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

        /// <summary>
        /// Show Details of a client
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns>Client</returns>
        public ActionResult Details(int id)
        {
            try
            {
                var client = clientManager.GetById(id);
                if (client != null)
                {
                    ClientViewModel model = new ClientViewModel
                    {
                        Id = client.Id,
                        Name = client.Name,
                        Description = client.Description,
                        ContactEmail = client.ContactEmail,
                        ContactName = client.ContactName,
                        ContactPhone = client.ContactPhone,
                        Url = client.Url,
                        ImagePath = client.ImagePath,
                        CreatedDate = client.CreatedDate,

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

        /// <summary>
        /// Method Create show creation form
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method Save Post Create form
        /// </summary>
        /// <param name="model">ClientViewModel</param>
        /// <param name="upload">File</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperUser")]
        public ActionResult Create(ClientViewModel model, IFormFile upload)
        {
            try
            {
                var file = Request.Form.Files["upload"];
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Images/Clients");
                var uniqueName = Guid.NewGuid().ToString()+"_"+file.FileName;
                using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }

                Client client = new Client
                {
                    Name = model.Name,
                    Description = model.Description,
                    ContactPhone= model.ContactPhone,
                    ContactName= model.ContactName,
                    ContactEmail= model.ContactEmail,
                    Url = model.Url,
                    ImagePath = "/Attachments/Images/Clients"+uniqueName,
                    CreatedDate = DateTime.Now,
                    UserId= User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                clientManager.AddAsync(client);
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

        /// <summary>
        /// Methos shwo edit form
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method save post edit form
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="model">Client</param>
        /// <returns></returns>
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

        /// <summary>
        /// Methos show delete page
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method confirms remove client
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="collection"></param>
        /// <returns></returns>
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

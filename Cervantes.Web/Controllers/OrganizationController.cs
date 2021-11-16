﻿using Cervantes.Contracts;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IOrganizationManager organizationManager = null;

        /// <summary>
        /// Organization Controller Constructor
        /// </summary>
        public OrganizationController(IOrganizationManager organizationManager, IHostingEnvironment _appEnvironment)
        {
            this.organizationManager = organizationManager;
            this._appEnvironment = _appEnvironment;

        }
        
        /// <summary>
        /// Method show Organization Information
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            try
            {

                var org = organizationManager.GetAll().FirstOrDefault();
                    
                 

                if (org != null)
                {
                    var model = new CORE.Organization
                    {
                        Id = org.Id,
                        Name = org.Name,
                        Description = org.Description,
                        ContactEmail = org.ContactEmail,
                        ContactName = org.ContactName,
                        ContactPhone = org.ContactPhone,
                        Url = org.Url,

                    };
                    return View(model);
                }
                else
                {
                    TempData["org"] = "No clients introduced";
                    return RedirectToAction("Edit");
                }


            }
            catch (Exception ex)
            {
                return View();
            }
        }



        /// <summary>
        /// Method show edit organization form
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            try
            {
                var result = organizationManager.GetAll().FirstOrDefault();

                if (result != null)
                {
                    var model = new OrganizationViewModel
                    {
                        Id = result.Id,
                        Name= result.Name,
                        ContactEmail = result.ContactEmail,
                        ContactName= result.ContactName,
                        ContactPhone= result.ContactPhone,
                        Url= result.Url,
                        ImagePath = result.ImagePath,
                        Description = result.Description,

                    };

                    return View(model);
                }
                else
                {
                    return View();
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
        /// Method save organization form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">OrganizationViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrganizationViewModel model)
        {

            try
            {
                var result = organizationManager.GetById(id);
                result.Name = model.Name;
                result.Description = model.Description;
                result.ContactEmail = model.ContactEmail;
                result.ContactName = model.ContactName;
                result.ContactPhone = model.ContactPhone;
                result.Url = model.Url;
                if (Request.Form.Files["upload"] != null)
                {
                    var file = Request.Form.Files["upload"];
                    var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Images/Avatars");
                    var uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    result.ImagePath = "/Attachments/Images/Organization/" + uniqueName;
                }


                organizationManager.Context.SaveChanges();
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
        /// Method delete organization logo
        /// </summary>
        /// <param name="id">Organization Id</param>
        /// <returns></returns>
        public ActionResult DeleteLogo(int id)
        {
            try
            {
                var org = organizationManager.GetById(id);
                var pathFile = _appEnvironment.WebRootPath + org.ImagePath;
                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                }


                org.ImagePath = null;
                organizationManager.Context.SaveChanges();

                TempData["avatar_deleted"] = "avatar deleted";
                return RedirectToAction("Edit", "Organization", new { id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Organization", new { id = id });
            }
        }

    }
}

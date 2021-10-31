using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IUserManager usrManager = null;
        IRoleManager roleManager = null;
        Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager = null;
        public UserController(IUserManager usrManager, IRoleManager roleManager, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IHostingEnvironment _appEnvironment)
        {
            this.usrManager = usrManager;
            this.roleManager = roleManager;
            this._appEnvironment = _appEnvironment;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            try
            {



                var model = usrManager.GetAll().Select(e => new UserViewModel
                {
                    Id = e.Id,
                    UserName = e.UserName,
                    FullName = e.FullName,
                    LockoutEnabled = e.LockoutEnabled,
                    TwoFactorEnabled = e.TwoFactorEnabled,

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

        // GET: HomeController1/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                var user = usrManager.GetByUserId(id);
                if (user != null)
                {
                    UserViewModel model = new UserViewModel
                    {
                        Id = user.Id,
                        UserName=user.UserName,
                        FullName=user.FullName,
                        TwoFactorEnabled=user.TwoFactorEnabled,
                        Email = user.Email,
                        Avatar  = user.Avatar,
                        Description = user.Description,
                        Position = user.Position,
                        PhoneNumber = user.PhoneNumber,
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

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model, IFormFile upload)
        {
            try
            {
                var file = Request.Form.Files["upload"];

                var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Images/Avatars");
                var uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;
                using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }


                var hasher = new PasswordHasher();

                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PasswordHash = hasher.HashPassword(model.Password),
                    FullName = model.FullName,
                    Avatar = "/Attachments/Images/Avatars" + uniqueName,
                    EmailConfirmed = true,
                    NormalizedEmail = model.Email.ToUpper(),
                    NormalizedUserName = model.UserName.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = model.PhoneNumber,
                    
                };
                usrManager.Add(user);
                usrManager.Context.SaveChanges();
                TempData["created"] = "created";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //guardamo log si hay un excepcion
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {

                //Obtenemos los roles y los pasamos a una lista
                var rol = roleManager.GetAll().Select(e => new RoleList
                {
                    Id = e.Id.ToString(),
                    Name = e.Name
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in rol)
                {
                    li.Add(new SelectListItem { Text = item.Name, Value = item.Id });
                }

                var user = usrManager.GetByUserId(id);
                var rolUser = _userManager.GetRolesAsync(user);
                var rolId = roleManager.GetByName(rolUser.Result.FirstOrDefault());


               

                if (user != null)
                {



                    var model = new UserEditViewModel
                    {
                        ItemList = li,
                        User = new UserViewModel
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            FullName = user.FullName,
                            TwoFactorEnabled = user.TwoFactorEnabled,
                            Email = user.Email,
                            Avatar = user.Avatar,
                            Description = user.Description,
                            Position = user.Position,
                            PhoneNumber = user.PhoneNumber,
                        },
                        Option = rolId.FirstOrDefault().Id
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

        // POST: HomeController1/Edit/5
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(string id)
        {
            try
            {
                var user = usrManager.GetByUserId(id);
                if (user != null)
                {
                    UserViewModel model = new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email,
                        Description = user.Description,
                        Position = user.Position,
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

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var user = usrManager.GetByUserId(id);
                if (user != null)
                {
                    usrManager.Remove(user);
                    usrManager.Context.SaveChanges();
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

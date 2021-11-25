using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.DAL;
using Cervantes.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        IUserManager usrManager = null;
        IProjectManager projectManager = null;
        IRoleManager roleManager = null;
        Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager = null;

        /// <summary>
        /// UserController Constructor
        /// </summary>
        /// <param name="usrManager">UserManager</param>
        /// <param name="roleManager">RoleManager</param>
        /// <param name="userManager">Identity UserManager</param>
        /// <param name="_appEnvironment">IHostingEnviroment</param>
        public UserController(IUserManager usrManager, IRoleManager roleManager, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IProjectManager projectManager, IHostingEnvironment _appEnvironment)
        {
            this.usrManager = usrManager;
            this.roleManager = roleManager;
            this._appEnvironment = _appEnvironment;
            _userManager = userManager;
            this.projectManager = projectManager;
        }

        /// <summary>
        /// Method show all users
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Method show user details
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            try
            {
                var user = usrManager.GetByUserId(id);
                var rolUser = _userManager.GetRolesAsync(user);

                if (user != null)
                {
                    UserViewModel model = new UserViewModel
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
                        Option = rolUser.Result.FirstOrDefault().ToString(),
                        Project = projectManager.GetAll().Where(x => x.UserId == user.Id)
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
        /// Mehtos show user creation form
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            try
            {
                //get roles to a list
                var rol = roleManager.GetAll().Select(e => new RoleList
                {
                    Id = e.Id.ToString(),
                    Name = e.Name
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in rol)
                {
                    li.Add(new SelectListItem { Text = item.Name, Value = item.Name });
                }

                var model = new UserViewModel
                {
                    ItemList = li,
                };

                return View(model);

            }
            catch
            {
                return View("Index");
            }

        }

        /// <summary>
        /// Method save user creation form
        /// </summary>
        /// <param name="model">UserViewModel</param>
        /// <param name="upload">Avatar File</param>
        /// <returns></returns>
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
                    Avatar = "/Attachments/Images/Avatars/" + uniqueName,
                    EmailConfirmed = true,
                    NormalizedEmail = model.Email.ToUpper(),
                    NormalizedUserName = model.UserName.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = model.PhoneNumber,
                    Position = model.Position,
                    Description = model.Description,

                };
                usrManager.Add(user);
                usrManager.Context.SaveChanges();
                _userManager.AddToRoleAsync(user, model.Option).Wait();




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
        /// Method show edit form
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            try
            {

                //get roles to a list
                var rol = roleManager.GetAll().Select(e => new RoleList
                {
                    Id = e.Id.ToString(),
                    Name = e.Name
                }).ToList();

                var li = new List<SelectListItem>();

                foreach (var item in rol)
                {
                    li.Add(new SelectListItem { Text = item.Name, Value = item.Name });
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
                        Option = rolUser.Result.FirstOrDefault()
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
        /// Method save edit form
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">UserEditViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserEditViewModel model)
        {
            try
            {

                var hasher = new PasswordHasher();
                var result = usrManager.GetByUserId(id);
                result.FullName = model.User.FullName;
                result.UserName = model.User.UserName;
                result.Email = model.User.Email;
                result.Description = model.User.Description;
                if (model.User.Password != null)
                {
                    result.PasswordHash = hasher.HashPassword(model.User.Password);
                }
                result.Position = model.User.Position;
                result.PhoneNumber = model.User.PhoneNumber;
                

                if (Request.Form.Files["upload"] != null)
                {
                    var file = Request.Form.Files["upload"];
                    var uploads = Path.Combine(_appEnvironment.WebRootPath, "Attachments/Images/Avatars");
                    var uniqueName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    using (var fileStream = new FileStream(Path.Combine(uploads, uniqueName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                    }
                    result.Avatar = "/Attachments/Images/Avatars/" + uniqueName;
                }

                usrManager.Context.SaveChanges();
                _userManager.AddToRoleAsync(result, model.Option).Wait();


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
        /// Method show delete page
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Method confirm delete user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="collection"></param>
        /// <returns></returns>
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


        public ActionResult DeleteAvatar(string id)
        {
            try
            {
                var user = usrManager.GetByUserId(id);
                var pathFile = _appEnvironment.WebRootPath+user.Avatar;
                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                }
                

                user.Avatar = null;
                usrManager.Context.SaveChanges();

                TempData["avatar_deleted"] = "avatar deleted";
                return RedirectToAction("Edit", "User", new { id = id });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "User", new { id = id });
            }
        }
    }

}
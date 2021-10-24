using Cervantes.Contracts;
using Cervantes.CORE;
using Cervantes.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cervantes.Web.Controllers
{
    public class UserController : Controller
    {
        IUserManager usrManager = null;
        IRoleManager roleManager = null;
        UserManager<ApplicationUser> _userManager = null;
        public UserController(IUserManager usrManager, IRoleManager roleManager, UserManager<ApplicationUser> userManager)
        {
            this.usrManager = usrManager;
            this.roleManager = roleManager;
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
        public ActionResult Details(int id)
        {
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

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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

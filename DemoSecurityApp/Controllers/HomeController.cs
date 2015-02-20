using DemoSecurityApp.Filters;
using DemoSecurityApp.Helpers;
using DemoSecurityApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoSecurityApp.Controllers
{
    public class HomeController : Controller
    {
        IService service;
        public HomeController()
        {
            service = new Service();
        }

        public ActionResult unauthorizedactivity()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getAllAccessibleFeatureControlIDJSON()
        {
            List<string> controlIDS = new List<string>();
            bool runPermissioning = false;
            if (PermissionManager.enablePermissioningSystem)
            {
                controlIDS = PermissionManager.getAccessibleFeatureControlID();
                runPermissioning = true;
            }
            JsonPermissionManager jper = new JsonPermissionManager() { enablePerm = runPermissioning, controlIDS = controlIDS };
            return Json(jper, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Index()
        //{
        //    var mesg = TempData["lgnFailedMessage"];
        //    ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

        //    return View();
        //}

        public ActionResult login()
        {
            PermissionManager asasd = PermissionManager.getPermissions();

            if (asasd != null)
                return RedirectToAction("userlist");

            var allUsers = service.GetAllUsers();
            return View(allUsers);
        }

        [HttpPost]
        public ActionResult login(int id)
        {
            int userid = id != 0 ? id : new Random().Next(1, Constants.USERSCOUNT);
            PermissionManager.setPermissions(service.GetUserAtIndex(userid));
            return RedirectToAction("userlist");
        }

        [PermissionFilter]
        public ActionResult UserList()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(service.GetAllUsers());
        }

        public ActionResult logout()
        {
            PermissionManager.logout();
            //return Content("Logged out successfully");
            return RedirectToAction("login");
        }

        [PermissionFilter]
        public ActionResult Edit()
        {
            ViewBag.Message = "Your Edit User page.";

            return View();
        }

        [PermissionFilter]
        public ActionResult Details()
        {
            ViewBag.Message = "Your User details page.";

            return View();
        }

        [PermissionFilter]
        public ActionResult Delete()
        {
            ViewBag.Message = "Your Delete User page.";

            return View();
        }
    }
}

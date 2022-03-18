using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.PORTAL.logics;

namespace FI.PORTAL.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserLogin()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return View();
        }

        public int UserLoginValidation(string uname, string password)
        {
            try
            {
                requestinit_logic logic = new requestinit_logic();
                int role = logic.UserLogin(uname, password);
                if (role > 0)
                {
                    Session["role"] = role.ToString();

                    Session["uname"] = uname.ToUpper();
                    return role;
                }
                return 0;
            }
            catch (Exception ex) { return 0; }
        }

    }
}
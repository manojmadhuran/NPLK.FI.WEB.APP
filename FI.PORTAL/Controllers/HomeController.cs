using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.PORTAL.logics;
using FI.PORTAL.Models;

namespace FI.PORTAL.Controllers
{
    public class HomeController : Controller
    {
        static private Int64 reqID_ = 0;
        public ActionResult Index()
        {
            try
            {
                int role = Convert.ToInt16(Session["role"].ToString());
                if (role > 0)
                {

                }
                else
                {
                    Response.Redirect("/Login/UserLogin", false);
                }
            }
            catch
            {
                Response.Redirect("/Login/UserLogin", false);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Initiation()
        {
           
            return View();
        }

        public ActionResult ViewRequests()
        {
            return View();
        }

        public ActionResult ViewComments()
        {           
            return View();
        }

        [HttpPost]
        public ActionResult viewComments_(long reqID)
        {
            try
            {                   
                    List<commentModel> commentList = new List<commentModel>();
                    requestinit_logic request = new requestinit_logic();
                    commentList = request.viewComments(reqID);                   
                    //return Json(commentList, JsonRequestBehavior.AllowGet);
                return Json(new { data = commentList, draw = Request["draw"], recordsTotal = commentList.Count, recordsFiltered = 1 }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult AddRequestComment(Int64 ReqID, string allowlimit, string comment, string Uaction,int Level)
        {
            try
            {
                string userx = Session["uname"].ToString();
                //string userx = "admin";
               
                requestinit_logic request = new requestinit_logic();
                Int64 reqID = request.AddRequestComment(ReqID, allowlimit, comment, "1", userx, Uaction, Level);
                
                return Json(reqID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult reasonList()
        {
            List<string> reason = new List<string>();
            try
            {
                requestinit_logic logic = new requestinit_logic();
                reason = logic.downloadreasons();
                return Json(reason, JsonRequestBehavior.AllowGet);
            }catch(Exception ex) { return null; }
        }
        
        [HttpGet]
        public JsonResult LevelList()
        {           
            List<string> Level = new List<string>();
            try
            {
                int role = Convert.ToInt16(Session["role"].ToString());
                //int role = 1;
                requestinit_logic logic = new requestinit_logic();
                Level = logic.downloadReqLevel(role);
                return Json(Level, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { return null; }
        }

        [HttpGet]
        public JsonResult LoadHistory(int reqid)
        {
            List<string> History = new List<string>();
            try
            {
                requestinit_logic logic = new requestinit_logic();
                History = logic.LoadHistory(reqid);
                return Json(History, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json("",JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadCusDetails(string cusCode, string filename)
        {
            try
            {
                requestinit_logic logic = new requestinit_logic();
                customerModel customer = logic.getCustomerBasicDetail(cusCode);
                return Json(customer,JsonRequestBehavior.AllowGet);

            }catch(Exception ex) { return Json("", JsonRequestBehavior.AllowGet); }
        }


        [HttpGet]
        public JsonResult InitiateRequest(string cuscode, string filename, string salescode, string allowlimit, string comment, string Rtype, 
            string crdExpo, string existing)
        {
            try
            {
                string user = Session["uname"].ToString();
                //string user = "admin";
                requestinit_logic request = new requestinit_logic();
                Int64 reqID = request.InitiateRequest(cuscode, filename, salescode, allowlimit, comment, user,Rtype, crdExpo, existing);
                Session["reqID_"] = reqID.ToString();
                return Json(reqID,JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult ViewRequests_(int cuscode, int year, int month, int status)
        {
            try
            {
                

                requestinit_logic request = new requestinit_logic();
               
                int role_ = Convert.ToInt16(Session["role"].ToString());
                string user_ = Session["uname"].ToString();
                //int role_ = 1;
                //string user_ = "admin";

                //Server Side Parameter
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchValue = Request["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortDirection = Request["order[0][dir]"];

                List<requestModel> model = request.viewRequest(cuscode, year, month, status, role_, user_);

                //Sorting    
                if (!string.IsNullOrEmpty(sortDirection))
                {
                    if (sortDirection.Equals("asc"))
                    {
                        model = model.OrderBy(a => a.Reqid).ToList();
                    }
                    else
                    {
                        model = model.OrderByDescending(a => a.Reqid).ToList();
                    }
                }

                int totalrows = model.Count;
                if (!string.IsNullOrEmpty(searchValue))//filter
                {
                    model = model.
                        Where(x => x.Cusname.ToLower().Contains(searchValue.ToLower()) || x.Crdlimit.ToLower().Contains(searchValue.ToLower()) || x.Curstatus.Equals(searchValue.ToLower())).ToList();
                }
                int totalrowsafterfiltering = model.Count;

                //paging
                model = model.Skip(start).Take(length).ToList();
                return Json(new { data = model, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
                

            }catch(Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult resultss()
        {
            return null;
        }


        [HttpPost]
        public void UploadFile( HttpPostedFileBase file)
        {
            try
            {
                string filename = Session["reqID_"].ToString();
                if (file == null)
                {
                    string _FileName = "404.png";                    
                    requestinit_logic logic = new requestinit_logic();
                    logic.saveFile(Convert.ToInt64(filename), _FileName);
                }
                else
                {
                    string _FileName = filename + Path.GetExtension(file.FileName);
                    //string _path = Path.Combine(Server.MapPath("~/Content/uploaded"), _FileName);
                    string _path = Path.Combine("//192.168.101.131/cmp/content/uploaded/", _FileName);
                    file.SaveAs(_path);
                    requestinit_logic logic = new requestinit_logic();
                    logic.saveFile(Convert.ToInt64(filename), _FileName);
                }
                Response.Redirect("/Home/Initiation",false);
                Session["reqID_"] = "";
            }
            catch(Exception ex) { }
        }




       

    }
}
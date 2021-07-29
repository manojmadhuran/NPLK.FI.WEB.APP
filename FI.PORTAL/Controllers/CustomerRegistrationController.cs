using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.PORTAL.dbconnect;
using FI.PORTAL.ViewModels;

namespace FI.PORTAL.Controllers
{
    public class CustomerRegistrationController : Controller
    {
        // GET: CustomerRequests
        CRP_SYSEntities CRP_dbObj = new CRP_SYSEntities();


        public ActionResult Index(string sortby)
        {
            if (sortby == null)
            {
                ViewBag.sortString = "All";
            }
            else if (sortby.Equals("Forwarded"))
            {
                ViewBag.sortString = "Forward/Reverse";
            }
            else if (sortby.Equals("Pending"))
            {
                ViewBag.sortString = "Pending";
            }
            else if (sortby.Equals("All"))
            {
                ViewBag.sortString = "All";
            }
            else
            {
                ViewBag.sortString = "Invalid";
            }


            int role = Convert.ToInt16(Session["role"].ToString());

            if (role == 1)
            {
                if (sortby != null)
                {
                    if (sortby.Equals("All"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5).ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Pending"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5 && r.CurrentStat == "pendingCO").ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Forwarded"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5 && r.CurrentStat != "pendingCO" && r.CurrentStat != "declineCO").ToList();
                        return View(res);
                    }
                    else
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5).ToList();
                        return View(res);
                    }
                }
                else
                {
                    var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5).ToList();
                    return View(res);
                }



            }
            else if (role == 5)
            {
                if (sortby != null)
                {
                    if (sortby.Equals("All"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6).ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Pending"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6 && r.CurrentStat == "pendingCM").ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Forwarded"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6 && r.CurrentStat != "pendingCM").ToList();
                        return View(res);
                    }
                    else
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6).ToList();
                        return View(res);
                    }
                }
                else
                {
                    var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6).ToList();
                    return View(res);
                }

            }
            else if (role == 6)
            {
                if (sortby != null)
                {
                    if (sortby.Equals("All"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7).ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Pending"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7 && r.CurrentStat == "pendingFM").ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Forwarded"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7 && r.CurrentStat != "pendingFM").ToList();
                        return View(res);
                    }
                    else
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7).ToList();
                        return View(res);
                    }
                }
                else
                {
                    var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7).ToList();
                    return View(res);
                }

            }
            else if (role == 7)
            {
                if (sortby != null)
                {
                    if (sortby.Equals("All"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8).ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Pending"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8 && r.CurrentStat == "pendingGM").ToList();
                        return View(res);
                    }
                    else if (sortby.Equals("Forwarded"))
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8 && r.CurrentStat != "pendgingGM").ToList();
                        return View(res);
                    }
                    else
                    {
                        var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8).ToList();
                        return View(res);
                    }
                }
                else
                {
                    var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8).ToList();
                    return View(res);
                }

            }
            else { return View(); }

        }

        public ActionResult CustomerRequestFormView(int CusReqID)
        {
            var Request = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.CusReqID == CusReqID).FirstOrDefault();
            var Companies = CRP_dbObj.NEW_CUS_COMPANIES.Where(r => r.CusReqID == CusReqID).ToList();
            var PaintBrands = CRP_dbObj.NEW_CUS_PAINT_BRANDS.Where(r => r.CusReqID == CusReqID).ToList();
            var Images = CRP_dbObj.NEW_CUS_IMAGE.Where(r => r.CusReqID == CusReqID).ToList();
            var Evaluations = CRP_dbObj.NEW_CUS_EVAL.Where(r => r.CusReqID == CusReqID).ToList();
            var FinalApproval = CRP_dbObj.NEW_CUS_FINAL_APPROVAL.Where(r => r.CusReqID == CusReqID).ToList();


            var viewModel = new CustomerRegistrationRequestData
            {
                RequestHeader = (NEW_CUS_HEADER)Request,
                RequestCompanies = Companies,
                RequestPaintBrands = PaintBrands,
                RequestImages = Images,
                RequestEvaluations = Evaluations,
                RequestFinalApproval = FinalApproval
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ReverseOrApproveRequest(CustomerRegistrationRequestData model)
        {


            NEW_CUS_EVAL newEvalObj = new NEW_CUS_EVAL();
            newEvalObj.EvalID = model.Evaluation.EvalID;
            newEvalObj.CusReqID = model.Evaluation.CusReqID;
            newEvalObj.RecLimit = model.Evaluation.RecLimit;
            newEvalObj.RecPeriod = model.Evaluation.RecPeriod;
            newEvalObj.RecDiscount = model.Evaluation.RecDiscount;
            newEvalObj.Remarks = model.Evaluation.Remarks;
            newEvalObj.Role = model.Evaluation.Role;
            newEvalObj.SalesCode = model.Evaluation.SalesCode;
            newEvalObj.Status = model.Evaluation.Status;
            newEvalObj.CreatedDate = DateTime.Now;

            CRP_dbObj.Entry(model.RequestHeader).State = System.Data.Entity.EntityState.Modified;
            CRP_dbObj.NEW_CUS_EVAL.Add(newEvalObj);

            CRP_dbObj.SaveChanges();

            int role = Convert.ToInt16(Session["role"].ToString());

            if (role == 1)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5).ToList();
                return View("Index", res);
            }
            else if (role == 5)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6).ToList();
                return View("Index", res);
            }
            else if (role == 6)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7).ToList();
                return View("Index", res);
            }
            else
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8).ToList();
                return View("Index", res);
            }

        }


        [HttpPost]
        public ActionResult ApproveRequest(CustomerRegistrationRequestData model)
        {

            NEW_CUS_FINAL_APPROVAL finalApprovalObj = new NEW_CUS_FINAL_APPROVAL();
            finalApprovalObj.ID = model.FinalApprove.ID;
            finalApprovalObj.CompletenessCustomerRegForm = model.FinalApprove.CompletenessCustomerRegForm;
            finalApprovalObj.AllRelevantDocCopies = model.FinalApprove.AllRelevantDocCopies;
            finalApprovalObj.FormA = model.FinalApprove.FormA;
            finalApprovalObj.FormB = model.FinalApprove.FormB;
            finalApprovalObj.DateofVerification = model.FinalApprove.DateofVerification;
            finalApprovalObj.PersonContacted = model.FinalApprove.PersonContacted;
            finalApprovalObj.MethodOfVerification = model.FinalApprove.MethodOfVerification;
            finalApprovalObj.Comments = model.FinalApprove.Comments;
            finalApprovalObj.CusReqID = model.FinalApprove.CusReqID;
            finalApprovalObj.RecLimit = model.FinalApprove.RecLimit;
            finalApprovalObj.RecPeriod = model.FinalApprove.RecPeriod;
            finalApprovalObj.RecDiscount = model.FinalApprove.RecDiscount;
            finalApprovalObj.Remarks = model.FinalApprove.Remarks;
            finalApprovalObj.Role = model.FinalApprove.Role;
            finalApprovalObj.SalesCode = model.FinalApprove.SalesCode;
            finalApprovalObj.CreatedDate = DateTime.Now;

            CRP_dbObj.Entry(model.RequestHeader).State = System.Data.Entity.EntityState.Modified;
            CRP_dbObj.NEW_CUS_FINAL_APPROVAL.Add(finalApprovalObj);

            CRP_dbObj.SaveChanges();

            int role = Convert.ToInt16(Session["role"].ToString());

            if (role == 1)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 5).ToList();
                return View("Index", res);
            }
            else if (role == 5)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 6).ToList();
                return View("Index", res);
            }
            else if (role == 6)
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 7).ToList();
                return View("Index", res);
            }
            else
            {
                var res = CRP_dbObj.NEW_CUS_HEADER.Where(r => r.Level >= 8).ToList();
                return View("Index", res);
            }

        }


    }
}
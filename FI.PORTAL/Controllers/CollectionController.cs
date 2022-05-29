using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FI.PORTAL.dbconnect;
using FI.PORTAL.Report;
using FI.PORTAL.ViewModels;
using iTextSharp.text.pdf;
using PagedList;

namespace FI.PORTAL.Controllers
{
    public class CollectionController : Controller
    {
        int pageSize = 10;
        // GET: Collection
        Collection_SYSEntities COL_dbObj = new Collection_SYSEntities();
        DateTime fromDateObj;
        DateTime toDateObj;
        public ActionResult CollectionHome(string sortby, int? page, string type)
        {
            if (Session["fromDate"] != null || Session["toDate"] != null)
            {
                String fromDate = Session["fromDate"].ToString();
                String toDate = Session["toDate"].ToString();
                fromDateObj = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                toDateObj = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                toDateObj = toDateObj.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                fromDateObj = new DateTime(1990, 01, 01);
                toDateObj = DateTime.Now;
            }


            if (Session["SearchBy"] == null)
            {
                Session["SearchBy"] = "";
            }

            if (Session["SearchValue"] == null)
            {
                Session["SearchValue"] = "";
            }

            if (type == "reset")
            {
                Session["SearchValue"] = "";
            }

            int pageNumber = (page ?? 1);
            //var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            if (Session["sorting"] == null)
            {
                //Session["selected_area"] = result_areas.FirstOrDefault().SalesCode;
                //var area = Session["selected_area"].ToString();
                Session["sorting"] = "All";
                ViewBag.sortString = "All";


                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj || toDateObj == null && c.collection_date >= fromDateObj || fromDateObj == null).ToList();


                var viewModel = new CollectionVM
                {
                    collection = result_collections.ToPagedList(pageNumber, pageSize),

                };


                return View(viewModel);
            }
            else
            {
                ViewBag.sortString = Session["sorting"].ToString();
                string sorting = Session["sorting"].ToString();
                //var area = Session["selected_area"].ToString();
                var result_collections = COL_dbObj.COLLECTIONS.ToList();

                if (sorting.Equals("All"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
                }
                else if (sorting.Equals("Pending"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
                }
                else if (sorting.Equals("Acknowledged"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
                }
                else if (sorting.Equals("Closed"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => (c.acknowledge == true && c.entered_to_sap == true) && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
                }

                var viewModel = new CollectionVM
                {
                    collection = result_collections.ToPagedList(pageNumber, pageSize),
                };

                return View(viewModel);
            }

        }

        public ActionResult CollectionView(string collection_no)
        {
            var CollectionData = COL_dbObj.FullCollectionData(collection_no).ToList();
            var Collection_Ticket = COL_dbObj.COLLECTION_TICKETS.Where(c => c.collection_id == collection_no).FirstOrDefault();

            if (Collection_Ticket == null)
            {
                var viewModel = new CollectionData
                {
                    FullCollectionData = CollectionData,
                    Ticket = Collection_Ticket,
                    Ticket_Msgs = null,
                    messageModel = new Models.MessageModel(),
                    ticketModel = new Models.TicketModel(),
                };

                return View(viewModel);
            }
            else
            {
                var Collection_Msgs = COL_dbObj.TICKET_MSGS.Where(c => c.ticket_id == Collection_Ticket.ticket_id);

                var viewModel = new CollectionData
                {
                    FullCollectionData = CollectionData,
                    Ticket = Collection_Ticket,
                    Ticket_Msgs = Collection_Msgs,
                    messageModel = new Models.MessageModel(),
                    ticketModel = new Models.TicketModel(),
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult AcknowledgeCollection(CollectionData model, int? page)
        {
            int pageNumber = (page ?? 1);
            var result = COL_dbObj.COLLECTIONS.Where(c => c.collection_no == model.Collection.collection_no).FirstOrDefault();
            result.id = model.Collection.id;
            result.acknowledge_by = model.Collection.acknowledge_by;
            result.acknowledge = model.Collection.acknowledge;
            result.acknowledged_date = DateTime.Now;

            COL_dbObj.SaveChanges();

            ////var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            //ViewBag.sortString = Session["sorting"].ToString();
            //string sorting = Session["sorting"].ToString();
            ////var area = Session["selected_area"].ToString();
            //var result_collections = COL_dbObj.COLLECTIONS.ToList();

            //if (sorting.Equals("All"))
            //{
            //    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
            //}
            //else if (sorting.Equals("Pending"))
            //{
            //    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
            //}
            //else if (sorting.Equals("Acknowledged"))
            //{
            //    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
            //}
            //else if (sorting.Equals("Closed"))
            //{
            //    result_collections = COL_dbObj.COLLECTIONS.Where(c => (c.acknowledge == true && c.entered_to_sap == true) && c.collection_date <= toDateObj && c.collection_date >= fromDateObj).ToList();
            //}


            //var viewModel = new CollectionVM
            //{
            //    collection = result_collections.ToPagedList(pageNumber, pageSize),

            //};

            return RedirectToAction("CollectionHome", "Collection");
        }



        public ActionResult SearchCollection(string search_query)
        {
            Session["searching"] = "true";
            //var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            ViewBag.sortString = Session["sorting"].ToString();
            string sorting = Session["sorting"].ToString();
            //var area = Session["selected_area"].ToString();
            var result_collections = (dynamic)null;

            if (sorting.Equals("All"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_no == search_query).ToList();
            }
            else if (sorting.Equals("Pending"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.collection_no == search_query).ToList();
            }
            else if (sorting.Equals("Acknowledged"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.collection_no == search_query).ToList();
            }

            var viewModel = new CollectionVM
            {
                collection = result_collections,
            };

            return View("CollectionHome", viewModel);
        }



        public ActionResult updateinvoice(string invid, bool update, string collection_id)
        {

            var res = COL_dbObj.UpdateInvoiceAcknowledge(invid, update).ToString();
            COL_dbObj.SaveChanges();

            var CollectionData = COL_dbObj.FullCollectionData(collection_id).ToList();

            var Collection_Ticket = COL_dbObj.COLLECTION_TICKETS.Where(c => c.collection_id == collection_id).FirstOrDefault();

            if (Collection_Ticket == null)
            {
                var viewModel = new CollectionData
                {
                    FullCollectionData = CollectionData,
                    Ticket = Collection_Ticket,
                    Ticket_Msgs = null,
                    messageModel = new Models.MessageModel(),
                    ticketModel = new Models.TicketModel(),
                };

                return View("CollectionView", viewModel);
            }
            else
            {
                var Collection_Msgs = COL_dbObj.TICKET_MSGS.Where(c => c.ticket_id == Collection_Ticket.ticket_id);

                var viewModel = new CollectionData
                {
                    FullCollectionData = CollectionData,
                    Ticket = Collection_Ticket,
                    Ticket_Msgs = Collection_Msgs,
                    messageModel = new Models.MessageModel(),
                    ticketModel = new Models.TicketModel(),
                };


                return View("CollectionView", viewModel);

            }
        }


        [HttpPost]
        public ActionResult sortAction(String sort)
        {
            Session["sorting"] = sort;
            return Json(new { msg = sort });
        }


        [HttpPost]
        public ActionResult areaAction(String area, String areaName)
        {
            Session["selected_area"] = area;
            Session["selected_area_name"] = areaName;
            return Json(new { msg = area });
        }

        [HttpPost]
        public ActionResult DateFilter(DateTime fromDate, DateTime toDate, String type)
        {
            if (type.Equals("reset"))
            {
                Session["fromDate"] = null;
                Session["toDate"] = null;
            }
            else
            {
                Session["fromDate"] = fromDate.ToString("yyyy-MM-dd");
                Session["toDate"] = toDate.ToString("yyyy-MM-dd");
            }

            return Json(new { msg = "success" });
        }

        public JsonResult SearchData(string SearchBy, string SearchValue, int? page)
        {
            int pageNumber = (page ?? 1);
            Session["sorting"] = "All";
            ViewBag.sortString = Session["sorting"].ToString();

            Session["SearchBy"] = SearchBy;
            Session["SearchValue"] = SearchValue;

            if (Session["fromDate"] != null || Session["toDate"] != null)
            {
                String fromDate = Session["fromDate"].ToString();
                String toDate = Session["toDate"].ToString();
                fromDateObj = DateTime.ParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                toDateObj = DateTime.ParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                toDateObj = toDateObj.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                fromDateObj = new DateTime(1990, 01, 01);
                toDateObj = DateTime.Now;
            }

            if (SearchBy.Equals("Sales_Code"))
            {
                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj && c.sales_code == SearchValue || SearchValue == null).ToList();
                return Json(result_collections);
            }
            else if (SearchBy.Equals("Collection_ID"))
            {
                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj && c.collection_no.Contains(SearchValue) || SearchValue == null).ToList();
                return Json(result_collections);
            }
            else if (SearchBy.Equals("Area"))
            {
                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj && c.area_name.Contains(SearchValue) || SearchValue == null).ToList();
                return Json(result_collections);
            }
            else if (SearchBy.Equals("Any"))
            {
                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj && c.area_name.Contains(SearchValue) || c.collection_no.Contains(SearchValue) || c.created_by.Contains(SearchValue) || SearchValue == null).ToList();
                return Json(result_collections);
            }
            else
            {
                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.collection_date <= toDateObj && c.collection_date >= fromDateObj && c.created_by.Contains(SearchValue) || SearchValue == null).ToList();
                return Json(result_collections);
            }

        }


        [HttpPost]
        public ActionResult TicketAction(Ticket ticket)
        {
            Collection_SYSEntities COL_dbObj = new Collection_SYSEntities();

            if (ticket.status.Equals("submit"))
            {
                if (ticket.Ticket_id == null)
                {
                    COLLECTION_TICKETS new_ticket = new COLLECTION_TICKETS();
                    new_ticket.collection_id = ticket.Collection_id;
                    new_ticket.status = true;
                    COL_dbObj.COLLECTION_TICKETS.Add(new_ticket);
                    COL_dbObj.SaveChanges();

                    var result = COL_dbObj.COLLECTION_TICKETS.Where(t => t.collection_id == ticket.Collection_id).FirstOrDefault();

                    TICKET_MSGS new_message = new TICKET_MSGS();
                    new_message.msg = ticket.Message;
                    new_message.ticket_id = result.ticket_id;
                    new_message.msg_from = ticket.from;
                    new_message.date_time = DateTime.Now;

                    COL_dbObj.TICKET_MSGS.Add(new_message);
                    COL_dbObj.SaveChanges();

                    return Json(new { msg = "Message Sent" });
                }
                else
                {
                    var result = COL_dbObj.COLLECTION_TICKETS.Where(t => t.collection_id == ticket.Collection_id).FirstOrDefault();
                    result.status = true;

                    TICKET_MSGS new_message = new TICKET_MSGS();
                    new_message.msg = ticket.Message;
                    new_message.msg_from = ticket.from;
                    new_message.ticket_id = Int32.Parse(ticket.Ticket_id);
                    new_message.date_time = DateTime.Now;

                    COL_dbObj.TICKET_MSGS.Add(new_message);
                    COL_dbObj.SaveChanges();

                    return Json(new { msg = "Message Sent" });
                }
            }
            else
            {
                var result = COL_dbObj.COLLECTION_TICKETS.Where(t => t.collection_id == ticket.Collection_id).FirstOrDefault();
                result.status = false;
                COL_dbObj.SaveChanges();

                return Json(new { msg = "Ticket Closed!" });
            }

        }



        [HttpPost]
        public ActionResult SAPAction(COLLECTION collection)
        {
            Collection_SYSEntities COL_dbObj = new Collection_SYSEntities();
            DateTime currentDateTime = DateTime.Now;

            var result = COL_dbObj.COLLECTIONS.Where(t => t.collection_no == collection.collection_no).FirstOrDefault();
            result.entered_to_sap = true;
            result.entered_by = collection.entered_by;
            result.sap_ref_no = collection.sap_ref_no;
            result.entered_date = currentDateTime;
         
            COL_dbObj.SaveChanges();

            return Json(new { msg = "Entered to SAP System Ref No" });


        }


        public ActionResult GenerateReport(string CollectionID)
        {

            int paymentCount = 0;
            var data = COL_dbObj.FullCollectionData(CollectionID).ToList();

            // old report for single paymnets only
            CollectionReport collectionReport = new CollectionReport();
            // new report for multiple payments only
            CollectionReportUpdate collectionReportUpdate = new CollectionReportUpdate();

            string uname = (Session["uname"].ToString());
            byte[] abytes;

            paymentCount = Int32.Parse(data.FirstOrDefault().PAYMENTS_COUNT.ToString());
            
            // if a multiple payment detected
            if (paymentCount > 1)
            {

                abytes = collectionReportUpdate.PrepareReport(data, uname, CollectionID);


            }
            else
            {

                abytes = collectionReport.PrepareReport(data, uname);

            }


            AddPageNumber addPageNumber = new AddPageNumber();


            return File(addPageNumber.PrepareReport(abytes), "application/pdf", "Collection_Report_" + data.FirstOrDefault().collection_no + ".pdf");
        }
    }
}

public class Ticket
{
    public string Collection_id { get; set; }
    public string Ticket_id { get; set; }
    public string Message { get; set; }
    public string status { get; set; }
    public string from { get; set; }

}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FI.PORTAL.dbconnect;
using FI.PORTAL.ViewModels;

namespace FI.PORTAL.Controllers
{
    public class CollectionController : Controller
    {
        // GET: Collection
        CollectionSYSEntities COL_dbObj = new CollectionSYSEntities();
        public ActionResult CollectionHome(string sortby)
        {
            var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            if (Session["sorting"] == null )
            {
                Session["selected_area"] = result_areas.FirstOrDefault().SalesCode;
                var area = Session["selected_area"].ToString();
                Session["sorting"] = "All";
                ViewBag.sortString = "All";

                var result_collections = COL_dbObj.COLLECTIONS.Where(c => c.sales_code == area).ToList();
           
                var viewModel = new CollectionVM
                {
                    collection = result_collections,
                    areas = result_areas
                };

                return View(viewModel);
            }
            else
            {
                ViewBag.sortString = Session["sorting"].ToString();
                string sorting = Session["sorting"].ToString();
                var area = Session["selected_area"].ToString();
                var result_collections = (dynamic)null; 
                if (sorting.Equals("All"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.sales_code == area).ToList();
                }
                else if (sorting.Equals("Pending"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.sales_code == area).ToList();
                }
                else if (sorting.Equals("Acknowledged"))
                {
                    result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.sales_code == area).ToList();
                }

                var viewModel = new CollectionVM
                {
                    collection = result_collections,
                    areas = result_areas
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
        public ActionResult AcknowledgeCollection(CollectionData model)
        {
            var result = COL_dbObj.COLLECTIONS.Where(c => c.collection_no == model.Collection.collection_no).FirstOrDefault();
            result.id = model.Collection.id;
            result.acknowledge_by = model.Collection.acknowledge_by;
            result.acknowledge = model.Collection.acknowledge;
            result.acknowledged_date = DateTime.Now;

            COL_dbObj.SaveChanges();

            var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            ViewBag.sortString = Session["sorting"].ToString();
            string sorting = Session["sorting"].ToString();
            var area = Session["selected_area"].ToString();
            var result_collections = (dynamic)null;

            if (sorting.Equals("All"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.sales_code == area).ToList();
            }
            else if (sorting.Equals("Pending"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.sales_code == area ).ToList();
            }
            else if (sorting.Equals("Acknowledged"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.sales_code == area ).ToList();
            }

            var viewModel = new CollectionVM
            {
                collection = result_collections,
                areas = result_areas
            };

            return View("CollectionHome", viewModel);
        }



        public ActionResult SearchCollection(string search_query)
        {
            var result_areas = COL_dbObj.Area_Master.OrderBy(a => a.AreaName).ToList();
            ViewBag.sortString = Session["sorting"].ToString();
            string sorting = Session["sorting"].ToString();
            var area = Session["selected_area"].ToString();
            var result_collections = (dynamic)null;

            if (sorting.Equals("All"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.sales_code == area && c.collection_no == search_query ).ToList();
            }
            else if (sorting.Equals("Pending"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false && c.sales_code == area && c.collection_no == search_query ).ToList();
            }
            else if (sorting.Equals("Acknowledged"))
            {
                result_collections = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true && c.sales_code == area && c.collection_no == search_query).ToList();
            }

            var viewModel = new CollectionVM
            {
                collection = result_collections,
                areas = result_areas
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
        public ActionResult TicketAction(Ticket ticket)
        {
            CollectionSYSEntities COL_dbObj = new CollectionSYSEntities();

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
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
            if (sortby == null)
            {
                ViewBag.sortString = "All";
                var res = COL_dbObj.COLLECTIONS.ToList();
                return View(res);
            }
            else if (sortby.Equals("Pending"))
            {
                ViewBag.sortString = "Pending";
                var res = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == false).ToList();
                return View(res);
            }
            else if (sortby.Equals("Acknowledged"))
            {
                ViewBag.sortString = "Acknowledged";
                var res = COL_dbObj.COLLECTIONS.Where(c => c.acknowledge == true).ToList();
                return View(res);
            }
            else if (sortby.Equals("All"))
            {
                ViewBag.sortString = "All";
                var res = COL_dbObj.COLLECTIONS.ToList();
                return View(res);
            }
            else
            {
                ViewBag.sortString = "Invalid";
                return View();
            }
        }

        public ActionResult CollectionView(string collection_no)
        {
            var CollectionData = COL_dbObj.FullCollectionData(collection_no).ToList();

            var viewModel = new CollectionData
            {
                FullCollectionData = CollectionData,
            };
            return View(viewModel);
        
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

            ViewBag.sortString = "All";
            var res = COL_dbObj.COLLECTIONS.ToList();
            return View("CollectionHome", res);
        }

        public ActionResult SearchCollection(string search_query)
        {
            ViewBag.sortString = "All";
            var res = COL_dbObj.COLLECTIONS.Where(c => c.collection_no == search_query).ToList();
            return View("CollectionHome", res);
        }
    }
}
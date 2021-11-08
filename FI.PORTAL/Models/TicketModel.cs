using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class TicketModel
    {
        public int ticket_id { get; set; }
        public string collection_id { get; set; }
        public bool status { get; set; }
    }
}
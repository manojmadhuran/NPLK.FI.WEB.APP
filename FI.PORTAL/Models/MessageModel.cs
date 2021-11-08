using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class MessageModel
    {
        public int msg_id { get; set; }
        public Nullable<int> ticket_id { get; set; }
        public string msg { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
    }
}
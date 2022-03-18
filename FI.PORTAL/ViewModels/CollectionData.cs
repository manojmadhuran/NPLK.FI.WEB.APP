using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FI.PORTAL.dbconnect;
using FI.PORTAL.Models;

namespace FI.PORTAL.ViewModels
{
    public class CollectionData
    {        
        public COLLECTION Collection { get; set; }

        public TicketModel ticketModel { get; set; }

        public MessageModel messageModel { get; set; }

        public COLLECTION_TICKETS Ticket { get; set; }

        public IEnumerable<TICKET_MSGS> Ticket_Msgs { get; set; }

        public IEnumerable<FullCollectionData_Result> FullCollectionData { get; set; }
    }
}
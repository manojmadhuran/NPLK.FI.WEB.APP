using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class commentModel
    {
        string userName, createdDate, comment, role, action, rtype;
        long commID;
        decimal limit;

        public string Role { get => role; set => role = value; }
        public string UserName { get => userName; set => userName = value; }
        public string CreatedDate { get => createdDate; set => createdDate = value; }
        public string Comment { get => comment; set => comment = value; }
        public string Action { get => action; set => action = value; }
        public decimal Limit { get => limit; set => limit = value; }
        public string RType { get => rtype; set => rtype = value; }
        public long CommID { get => commID; set => commID = value; }

    }
}
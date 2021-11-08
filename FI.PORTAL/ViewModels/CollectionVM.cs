using FI.PORTAL.dbconnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.ViewModels
{
    public class CollectionVM
    {
        public IEnumerable<COLLECTION> collection { get; set; }

        public IEnumerable<Area_Master> areas { get; set; }
    }
}
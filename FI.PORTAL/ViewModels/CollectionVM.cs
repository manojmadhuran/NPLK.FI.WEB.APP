using FI.PORTAL.dbconnect;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.ViewModels
{
    public class CollectionVM
    {
        public IPagedList<COLLECTION> collection { get; set; }


    }
}
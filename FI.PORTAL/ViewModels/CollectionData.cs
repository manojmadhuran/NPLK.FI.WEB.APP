using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FI.PORTAL.dbconnect;

namespace FI.PORTAL.ViewModels
{
    public class CollectionData
    {
        public COLLECTION Collection { get; set; }

        public IEnumerable<FullCollectionData_Result> FullCollectionData { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FI.PORTAL.dbconnect;

namespace FI.PORTAL.ViewModels
{
    public class CustomerRegistrationRequestData
    {
        public viewNEW_CUS_HEADER RequestHeader { get; set; }

        public IEnumerable<NEW_CUS_COMPANIES> RequestCompanies { get; set; }

        public IEnumerable<NEW_CUS_PAINT_BRANDS> RequestPaintBrands { get; set; }

        public IEnumerable<NEW_CUS_IMAGE> RequestImages { get; set; }

        public IEnumerable<NEW_CUS_EVAL> RequestEvaluations { get; set; }

        public IEnumerable<NEW_CUS_OWNERS> RequestOwners { get; set; }

        public IEnumerable<NEW_CUS_FINAL_APPROVAL> RequestFinalApproval { get; set; }

        public NEW_CUS_EVAL Evaluation { get; set; }

        public NEW_CUS_FINAL_APPROVAL FinalApprove {get; set;}
    }
}
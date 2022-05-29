using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class evaluationModel
    {
        // ------------------------------------ Model class for New Evaluation History --------------------- UpdateCode - FIU02
        string evalID, recPeriod, recDiscount, recLimit, remarks, salesCode, createdDate, status, cusReqID, rEPName;
        
        string role;

        public string RecLimit { get => recLimit; set => recLimit = value; }
        public string EvalID { get => evalID; set => evalID = value; }
        public string RecPeriod { get => recPeriod; set => recPeriod = value; }
        public string RecDiscount { get => recDiscount; set => recDiscount = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public string Role { get => role; set => role = value; }
        public string SalesCode { get => salesCode; set => salesCode = value; }    
        public string CreatedDate { get => createdDate; set => createdDate = value; }
        public string Status { get => status; set => status = value; }
        public string CusReqID { get => cusReqID; set => cusReqID = value; }
        public string REPName { get => rEPName; set => rEPName = value; }


    }
}
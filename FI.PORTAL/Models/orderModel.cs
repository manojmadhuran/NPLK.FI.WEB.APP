using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class orderModel
    {
        private string ordtype, status, areaname, createdby;
        private DateTime createddate;
        private int docid;  

        public string Ordtype { get => ordtype; set => ordtype = value; }
        public string Status { get => status; set => status = value; }
        public string Areaname { get => areaname; set => areaname = value; }
        public string Createdby { get => createdby; set => createdby = value; }
        public DateTime Createddate { get => createddate; set => createddate = value; }
        public int Docid { get => docid; set => docid = value; }
    }   
}
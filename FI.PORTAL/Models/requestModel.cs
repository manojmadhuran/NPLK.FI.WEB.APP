using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class requestModel
    {
        private string reqID,cuscode,cusname, crdlimit, currentlevel, comments, curstatus,reqstatus, date, fileName, Rtype,Salecode, saplimit;
        private DateTime createdon;
        private int count_,statusID;

        public string Reqid { get => reqID; set => reqID = value; }
        public string Cuscode { get => cuscode; set => cuscode = value; }
        public string Cusname { get => cusname; set => cusname = value; }
        public string Crdlimit { get => crdlimit; set => crdlimit = value; }
        public string Currentlevel { get => currentlevel; set => currentlevel = value; }
        public DateTime Createdon { get => createdon; set => createdon = value; }
        public string Date { get => date; set => date = value; }
        public string Comments { get => comments; set => comments = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string Curstatus { get => curstatus; set => curstatus = value; }
        public string Reqstatus { get => reqstatus; set => reqstatus = value; }
        public int Count_ { get => count_; set => count_ = value; }
        public int StatusID { get => statusID; set => statusID = value; }
        public string RType { get => Rtype; set => Rtype = value; }
        public string SaleCode { get => Salecode; set => Salecode = value; }
        public string SAPlimit { get => saplimit; set => saplimit = value; }
    }
}
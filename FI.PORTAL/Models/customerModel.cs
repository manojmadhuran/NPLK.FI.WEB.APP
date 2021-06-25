using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class customerModel
    {
        private string cusname, salescode, crdlimit, salesarea;
        private int cusid;

        public int Cusid { get => cusid; set => cusid = value; }
        public string Cusname { get => cusname; set => cusname = value; }
        public string SalesCode { get => salescode; set => salescode = value; }
        public string Crdlimit { get => crdlimit; set => crdlimit = value; }
        public string SalesArea { get => salesarea; set => salesarea = value; }
    }
}
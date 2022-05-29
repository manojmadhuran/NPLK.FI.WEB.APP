using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Models
{
    public class CollectionModal
    {

        String payment_type = "", cheque_no = "", issued_bank = "", issued_branch = "",
            deposited_bank = "", deposited_branch = "", receipt_no = "";
        Double payment_amount = 0, unallocated_amount = 0;
        DateTime banking_date;

        DateTime invoice_date;
        String invoice_no = "", payment_term = "", remarks = "";
        Double invoice_amount = 0, os_balance = 0, invoice_allocated = 0;
        int nod = 0;
        Boolean acknowledge;

        String customer_code = "" , CusName = "";

        public CollectionModal(string payment_type, string cheque_no, string issued_bank, string issued_branch, string deposited_bank, string deposited_branch, string receipt_no, double payment_amount, double unallocated_amount, DateTime banking_date, String customer_code, String CusName)
        {
            this.payment_type = payment_type;
            this.cheque_no = cheque_no;
            this.issued_bank = issued_bank;
            this.issued_branch = issued_branch;
            this.deposited_bank = deposited_bank;
            this.deposited_branch = deposited_branch;
            this.receipt_no = receipt_no;
            this.payment_amount = payment_amount;
            this.unallocated_amount = unallocated_amount;
            this.banking_date = banking_date;
            this.customer_code = customer_code;
            this.CusName = CusName;
        }

        public CollectionModal(DateTime invoice_date, string invoice_no, string payment_term, string remarks, Boolean acknowledge, double invoice_amount, double os_balance, double invoice_allocated, int nod)
        {
            this.invoice_date = invoice_date;
            this.invoice_no = invoice_no;
            this.payment_term = payment_term;
            this.remarks = remarks;
            this.acknowledge = acknowledge;
            this.invoice_amount = invoice_amount;
            this.os_balance = os_balance;
            this.invoice_allocated = invoice_allocated;
            this.nod = nod;
        }

        public CollectionModal()
        {
        }

        public string Payment_type { get => payment_type; set => payment_type = value; }
        public string Cheque_no { get => cheque_no; set => cheque_no = value; }
        public string Issued_bank { get => issued_bank; set => issued_bank = value; }
        public string Issued_branch { get => issued_branch; set => issued_branch = value; }
        public string Deposited_bank { get => deposited_bank; set => deposited_bank = value; }
        public string Deposited_branch { get => deposited_branch; set => deposited_branch = value; }
        public string Receipt_no { get => receipt_no; set => receipt_no = value; }        
        public DateTime Banking_date { get => banking_date; set => banking_date = value; }
        public double Payment_amount { get => payment_amount; set => payment_amount = value; }
        public double Unallocated_amount { get => unallocated_amount; set => unallocated_amount = value; }
        
        public DateTime Invoice_date { get => invoice_date; set => invoice_date = value; }
        public string Invoice_no { get => invoice_no; set => invoice_no = value; }
        public string Payment_term { get => payment_term; set => payment_term = value; }
        public string Remarks { get => remarks; set => remarks = value; }
        public Boolean Acknowledge { get => acknowledge; set => acknowledge = value; }
        public double Os_balance { get => os_balance; set => os_balance = value; }
        public double Invoice_amount { get => invoice_amount; set => invoice_amount = value; }
        public double Invoice_allocated { get => invoice_allocated; set => invoice_allocated = value; }
        public int Nod { get => nod; set => nod = value; }
        public string Customer_code { get => customer_code; set => customer_code = value; }
        public string CusName1 { get => CusName; set => CusName = value; }
    }
}
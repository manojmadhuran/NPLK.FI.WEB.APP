using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FI.PORTAL.dbconnect;
using FI.PORTAL.ViewModels;
using FI.PORTAL.Models;
using FI.PORTAL.logics;

namespace FI.PORTAL.Report
{
    public class CollectionReportUpdate
    {
        #region Declaration
        Document document;
        Font fontStyle, fontStyleBold, fontStyleNormal, heading, subheading;
        PdfPTable main_table = new PdfPTable(22);
        float[] main_table_widths = new float[] { 10, 15, 10, 8, 10, 10, 10, 10, 10, 8, 10, 10, 10, 10, 10, 10, 10, 10, 5, 10, 10, 10 };
        PdfPTable details_table = new PdfPTable(4);
        PdfPTable docdetails = new PdfPTable(1);
        PdfPTable header_table = new PdfPTable(1);
        PdfPTable subheader_table = new PdfPTable(1);
        PdfPTable outer = new PdfPTable(3);

        PdfPCell pdfPCell;
        int headerCellPadding = 3;
        MemoryStream memoryStream = new MemoryStream();

        List<FullCollectionData_Result> data = new List<FullCollectionData_Result>();


        //for new Calculations 
        static decimal initialChequeValue;
        static decimal chequeRemainValue;



        #endregion

        public byte[] PrepareReport(List<FullCollectionData_Result> crrd, string username, String CollectionID)
        {
            data = crrd;
            #region
            document = new Document(PageSize.LEGAL.Rotate(), 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.LEGAL.Rotate());
            document.SetMargins(10f, 10f, 20f, 20f);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            #endregion


            this.HeaderTable();
            this.SubheaderTable("Sales Collection Summary Report");
            this.MainTable(CollectionID);
            this.DetailsTable();
            this.DocTable(username);

            details_table.SpacingBefore = 10f;
            details_table.SpacingAfter = 12.5f;
            main_table.SpacingBefore = 10f;
            main_table.SpacingAfter = 12.5f;

            document.Add(header_table);
            document.Add(subheader_table);

            float[] details_table_widths = new float[] { 8, 8, 8, 10 };
            details_table.SetWidths(details_table_widths);


            float[] doc_table_widths = new float[] { 8, 8, 2 };
            outer.SetWidths(doc_table_widths);

            this.outerTable(details_table, docdetails);

            document.Add(outer);

            main_table.SetWidths(main_table_widths);
            document.Add(main_table);

            document.Close();

            return memoryStream.ToArray();

        }

        // Header topic Function
        private void HeaderTable()
        {
            header_table.WidthPercentage = 100;
            header_table.HorizontalAlignment = Element.ALIGN_CENTER;

            heading = FontFactory.GetFont("Tahoma", 16, 1);
            int cellPadding = 5;
            int borderSize = 0;

            #region Table body

            pdfPCell = new PdfPCell(new Phrase("Nippon Paint Lanka (PVT) LTD", heading));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            header_table.AddCell(pdfPCell);

            header_table.CompleteRow();
            #endregion
        }

        // Sub header function
        private void SubheaderTable(string title)
        {
            subheader_table.WidthPercentage = 100;
            subheader_table.HorizontalAlignment = Element.ALIGN_LEFT;
            subheader_table.DeleteRow(0);

            subheading = FontFactory.GetFont("Tahoma", 14, 1);
            int borderSize = 0;

            #region Table body

            pdfPCell = new PdfPCell(new Phrase(title, subheading));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.PaddingBottom = 20;
            pdfPCell.ExtraParagraphSpace = 0;
            subheader_table.AddCell(pdfPCell);

            subheader_table.CompleteRow();
            #endregion
        }

        // Collection Main details table in left corner
        private void DetailsTable()
        {
            details_table.WidthPercentage = 50;
            details_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header
            fontStyle = FontFactory.GetFont("Calibri", 8f, 1);

            pdfPCell = new PdfPCell(new Phrase("Collection ID: " + data.FirstOrDefault().collection_no, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            details_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Sales Area Code: " + data.FirstOrDefault().sales_code, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            details_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Sales Area: " + data.FirstOrDefault().area_name, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            details_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Rep Name: " + data.FirstOrDefault().created_by, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            details_table.AddCell(pdfPCell);

            details_table.CompleteRow();



            pdfPCell = new PdfPCell(new Phrase("Date: " + data.FirstOrDefault().collection_date.Value.ToShortDateString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.Colspan = 4;
            pdfPCell.ExtraParagraphSpace = 0;
            details_table.AddCell(pdfPCell);


            details_table.CompleteRow();
            #endregion

        }

        // Report details table in right corner
        private void DocTable(string username)
        {
            docdetails.WidthPercentage = 20;
            docdetails.HorizontalAlignment = Element.ALIGN_RIGHT;

            #region Table header
            fontStyle = FontFactory.GetFont("Calibri", 8f, 1);

            pdfPCell = new PdfPCell(new Phrase("Date: " + DateTime.Now.ToShortDateString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            docdetails.AddCell(pdfPCell);

            docdetails.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Time: " + DateTime.Now.ToShortTimeString(), fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            docdetails.AddCell(pdfPCell);

            docdetails.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("User: " + username, fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            docdetails.AddCell(pdfPCell);

            docdetails.CompleteRow();

            #endregion

        }

        private void outerTable(PdfPTable table1, PdfPTable table2)
        {
            outer.WidthPercentage = 100;
            outer.HorizontalAlignment = Element.ALIGN_RIGHT;

            #region Table header

            pdfPCell = new PdfPCell(new PdfPTable(table1));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.BorderColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            outer.AddCell(pdfPCell);


            pdfPCell = new PdfPCell();
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.BorderColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            outer.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new PdfPTable(table2));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.BorderColor = BaseColor.WHITE;
            pdfPCell.Padding = 3;
            pdfPCell.ExtraParagraphSpace = 0;
            outer.AddCell(pdfPCell);

            outer.CompleteRow();

            #endregion

        }

        // main table data loading function
        private void MainTable(String CollectionID)
        {

            main_table.WidthPercentage = 100;
            main_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header
            fontStyle = FontFactory.GetFont("Calibri", 6f, 1);

            pdfPCell = new PdfPCell(new Phrase("Dealer Code", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Dealer Name", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Payment Method", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Cheque No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Bank", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Branch", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Banking Date", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Deposited Bank", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Deposited Branch", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Receipt No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Amount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Unallocated Amount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Invoice Date", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Invoice No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Payment Term", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Amount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("OS Balance", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Settled Amount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("NOD", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Remarks", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Acknowledge", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("System Reference", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);

            main_table.CompleteRow();
            #endregion

            #region Table Body

            fontStyle = FontFactory.GetFont("Tahoma", 6f, 0);


            decimal amountTotal = 0;
            decimal unallocatedAmountTotal = 0;
            decimal invoiceAmountTotal = 0;
            decimal osBalanceTotal = 0;
            decimal settledTotal = 0;
            DateTime nulldate = new DateTime(0001, 1, 1);
            List<String> multiplePaymentKeyList = new List<string> { };
            List<String> multiplePaymentCheques = new List<string> { };
            List<String> multiplePaymentInvoices = new List<string> { };

            // New Method to Print the report 
            int counter = 0;
            int inverseCounter = 0;
            universalController uc = new universalController();
            List<CollectionModal> chequeDetails = uc.GetChequeDetailsForReport(CollectionID);
            List<CollectionModal> PayedInvoiceDetails = uc.GetPayedInvoicesForReport(CollectionID);
            List<CollectionModal> ModalData = new List<CollectionModal> { };


            if (chequeDetails.Count == PayedInvoiceDetails.Count)
            {
                counter = chequeDetails.Count;
                inverseCounter = chequeDetails.Count;
                
            }
            else if (chequeDetails.Count > PayedInvoiceDetails.Count)
            {

                counter = chequeDetails.Count;
                inverseCounter = PayedInvoiceDetails.Count;
               

            }
            else if (chequeDetails.Count < PayedInvoiceDetails.Count)
            {
                counter = PayedInvoiceDetails.Count;
                inverseCounter = chequeDetails.Count;
                
            }

            // if cheques and invoices are equal
            if (counter == inverseCounter) {

                // this loop will merge both Modal objects from two list in to single one
                for (int i = 0; i < inverseCounter; i++)
                {

                    CollectionModal collectionModal = new CollectionModal();

                    // get the cheque data from the small array first
                    collectionModal.Payment_type = chequeDetails[i].Payment_type;
                    collectionModal.Cheque_no = chequeDetails[i].Cheque_no;
                    collectionModal.Issued_bank = chequeDetails[i].Issued_bank;
                    collectionModal.Issued_branch = chequeDetails[i].Issued_branch;
                    collectionModal.Deposited_bank = chequeDetails[i].Deposited_bank;
                    collectionModal.Deposited_branch = chequeDetails[i].Deposited_branch;
                    collectionModal.Receipt_no = chequeDetails[i].Receipt_no;
                    collectionModal.Payment_amount = chequeDetails[i].Payment_amount;
                    collectionModal.Unallocated_amount = chequeDetails[i].Unallocated_amount;
                    collectionModal.Banking_date = chequeDetails[i].Banking_date;

                    collectionModal.Invoice_date = PayedInvoiceDetails[i].Invoice_date;
                    collectionModal.Invoice_no = PayedInvoiceDetails[i].Invoice_no;
                    collectionModal.Payment_term = PayedInvoiceDetails[i].Payment_term;
                    collectionModal.Remarks = PayedInvoiceDetails[i].Remarks;
                    collectionModal.Invoice_amount = PayedInvoiceDetails[i].Invoice_amount;
                    collectionModal.Os_balance = PayedInvoiceDetails[i].Os_balance;
                    collectionModal.Invoice_allocated = PayedInvoiceDetails[i].Invoice_allocated;
                    collectionModal.Nod = PayedInvoiceDetails[i].Nod;
                    collectionModal.Acknowledge = PayedInvoiceDetails[i].Acknowledge;

                    collectionModal.Customer_code = chequeDetails[i].Customer_code;
                    collectionModal.CusName1 = chequeDetails[i].CusName1;

                    ModalData.Add(collectionModal);
                }
              

            }// if number of cheques are lesser than number of invoices
            else if (inverseCounter < counter)
            {

                // this loop will merge both Modal objects from two list in to single one and add to another modal list
                for (int i = 0; i < inverseCounter; i++)
                {

                    CollectionModal collectionModal = new CollectionModal();

                    // get the cheque data from the small array first
                    collectionModal.Payment_type = chequeDetails[i].Payment_type;
                    collectionModal.Cheque_no = chequeDetails[i].Cheque_no;
                    collectionModal.Issued_bank = chequeDetails[i].Issued_bank;
                    collectionModal.Issued_branch = chequeDetails[i].Issued_branch;
                    collectionModal.Deposited_bank = chequeDetails[i].Deposited_bank;
                    collectionModal.Deposited_branch = chequeDetails[i].Deposited_branch;
                    collectionModal.Receipt_no = chequeDetails[i].Receipt_no;
                    collectionModal.Payment_amount = chequeDetails[i].Payment_amount;
                    collectionModal.Unallocated_amount = chequeDetails[i].Unallocated_amount;
                    collectionModal.Banking_date = chequeDetails[i].Banking_date;

                    collectionModal.Invoice_date = PayedInvoiceDetails[i].Invoice_date;
                    collectionModal.Invoice_no = PayedInvoiceDetails[i].Invoice_no;
                    collectionModal.Payment_term = PayedInvoiceDetails[i].Payment_term;
                    collectionModal.Remarks = PayedInvoiceDetails[i].Remarks;
                    collectionModal.Invoice_amount = PayedInvoiceDetails[i].Invoice_amount;
                    collectionModal.Os_balance = PayedInvoiceDetails[i].Os_balance;
                    collectionModal.Invoice_allocated = PayedInvoiceDetails[i].Invoice_allocated;
                    collectionModal.Nod = PayedInvoiceDetails[i].Nod;
                    collectionModal.Acknowledge = PayedInvoiceDetails[i].Acknowledge;

                    collectionModal.Customer_code = chequeDetails[i].Customer_code;
                    collectionModal.CusName1 = chequeDetails[i].CusName1;

                    ModalData.Add(collectionModal);
                }

                // this loop will add the remainging modals to the same list 
                for (int j = inverseCounter; j < counter; j++)
                {

                    CollectionModal collectionModal = new CollectionModal();


                    collectionModal.Invoice_date = PayedInvoiceDetails[j].Invoice_date;
                    collectionModal.Invoice_no = PayedInvoiceDetails[j].Invoice_no;
                    collectionModal.Payment_term = PayedInvoiceDetails[j].Payment_term;
                    collectionModal.Remarks = PayedInvoiceDetails[j].Remarks;
                    collectionModal.Invoice_amount = PayedInvoiceDetails[j].Invoice_amount;
                    collectionModal.Os_balance = PayedInvoiceDetails[j].Os_balance;
                    collectionModal.Invoice_allocated = PayedInvoiceDetails[j].Invoice_allocated;
                    collectionModal.Nod = PayedInvoiceDetails[j].Nod;
                    collectionModal.Acknowledge = PayedInvoiceDetails[j].Acknowledge;
                    collectionModal.Customer_code = PayedInvoiceDetails[j].Customer_code;
                    collectionModal.CusName1 = PayedInvoiceDetails[j].CusName1;

                    ModalData.Add(collectionModal);
                }
            }// if number of cheques are greater than number of invoices
            else if (counter > inverseCounter) {

                // this loop will merge both Modal objects from two list in to single one 
                for (int i = 0; i < inverseCounter; i++)
                {

                    CollectionModal collectionModal = new CollectionModal();

                    // get the cheque data from the small array first
                    collectionModal.Payment_type = chequeDetails[i].Payment_type;
                    collectionModal.Cheque_no = chequeDetails[i].Cheque_no;
                    collectionModal.Issued_bank = chequeDetails[i].Issued_bank;
                    collectionModal.Issued_branch = chequeDetails[i].Issued_branch;
                    collectionModal.Deposited_bank = chequeDetails[i].Deposited_bank;
                    collectionModal.Deposited_branch = chequeDetails[i].Deposited_branch;
                    collectionModal.Receipt_no = chequeDetails[i].Receipt_no;
                    collectionModal.Payment_amount = chequeDetails[i].Payment_amount;
                    collectionModal.Unallocated_amount = chequeDetails[i].Unallocated_amount;
                    collectionModal.Banking_date = chequeDetails[i].Banking_date;

                    collectionModal.Invoice_date = PayedInvoiceDetails[i].Invoice_date;
                    collectionModal.Invoice_no = PayedInvoiceDetails[i].Invoice_no;
                    collectionModal.Payment_term = PayedInvoiceDetails[i].Payment_term;
                    collectionModal.Remarks = PayedInvoiceDetails[i].Remarks;
                    collectionModal.Invoice_amount = PayedInvoiceDetails[i].Invoice_amount;
                    collectionModal.Os_balance = PayedInvoiceDetails[i].Os_balance;
                    collectionModal.Invoice_allocated = PayedInvoiceDetails[i].Invoice_allocated;
                    collectionModal.Nod = PayedInvoiceDetails[i].Nod;
                    collectionModal.Acknowledge = PayedInvoiceDetails[i].Acknowledge;

                    collectionModal.Customer_code = chequeDetails[i].Customer_code;
                    collectionModal.CusName1 = chequeDetails[i].CusName1;

                    ModalData.Add(collectionModal);
                }
                // this loop will add the remainging modals to the same list 
                for (int j = inverseCounter; j < counter; j++)
                {

                    CollectionModal collectionModal = new CollectionModal();


                    // get the cheque data from the large array next
                    collectionModal.Payment_type = chequeDetails[j].Payment_type;
                    collectionModal.Cheque_no = chequeDetails[j].Cheque_no;
                    collectionModal.Issued_bank = chequeDetails[j].Issued_bank;
                    collectionModal.Issued_branch = chequeDetails[j].Issued_branch;
                    collectionModal.Deposited_bank = chequeDetails[j].Deposited_bank;
                    collectionModal.Deposited_branch = chequeDetails[j].Deposited_branch;
                    collectionModal.Receipt_no = chequeDetails[j].Receipt_no;
                    collectionModal.Payment_amount = chequeDetails[j].Payment_amount;
                    collectionModal.Unallocated_amount = chequeDetails[j].Unallocated_amount;
                    collectionModal.Banking_date = chequeDetails[j].Banking_date;

                    ModalData.Add(collectionModal);
                }


            }

            if (counter > 0)
            {
                pdfPCell = new PdfPCell(new Phrase("MULTIPLE PAYMENT", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                pdfPCell.Colspan = 23;
                main_table.AddCell(pdfPCell);
                main_table.CompleteRow();

                foreach (var item in ModalData)
                {



                    pdfPCell = new PdfPCell(new Phrase(item.Customer_code == null ? "" : item.Customer_code, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.CusName1 == null ? "" : item.CusName1, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);




                    pdfPCell = new PdfPCell(new Phrase(item.Payment_type == null ? "" : item.Payment_type, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Cheque_no == null ? "" : item.Cheque_no, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Issued_bank == null ? "" : item.Issued_bank, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Issued_branch == null ? "" : item.Issued_branch, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Banking_date == nulldate ? "" : item.Banking_date.ToShortDateString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Deposited_bank == null ? "" : item.Deposited_bank, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Deposited_branch == null ? "" : item.Deposited_branch, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Receipt_no == null ? "" : item.Receipt_no, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Payment_amount == 0 ? "" : Convert.ToDecimal(item.Payment_amount).ToString("#,##0.00"), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Unallocated_amount == 0 ? "" : Convert.ToDecimal(item.Unallocated_amount).ToString("#,##0.00"), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Invoice_date == nulldate ? "" : item.Invoice_date.ToShortDateString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Invoice_no == null ? "" : item.Invoice_no, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Payment_term == null ? "" : item.Payment_term, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Invoice_amount == 0 ? "" : Convert.ToDecimal(item.Invoice_amount).ToString("#,##0.00"), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Os_balance == 0 ? "" : Convert.ToDecimal(item.Os_balance).ToString("#,##0.00"), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Invoice_allocated == 0 ? "" : Convert.ToDecimal(item.Invoice_allocated).ToString("#,##0.00"), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Nod == 0 ? "" : item.Nod.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Remarks == null ? "" : item.Remarks, fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Acknowledge == true ? "Yes" : "No", fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.Padding = headerCellPadding;
                    pdfPCell.ExtraParagraphSpace = 0;
                    main_table.AddCell(pdfPCell);

                    main_table.CompleteRow();

                    if (Convert.ToDecimal(item.Payment_amount) != 0)
                    {
                        amountTotal = amountTotal + Convert.ToDecimal(item.Payment_amount);
                    }
                    if (Convert.ToDecimal(item.Os_balance) != 0)
                    {
                        osBalanceTotal = osBalanceTotal + Convert.ToDecimal(item.Os_balance);
                    }
                    if (Convert.ToDecimal(item.Invoice_allocated) != 0)
                    {
                        settledTotal = settledTotal + Convert.ToDecimal(item.Invoice_allocated);
                    }
                    if (Convert.ToDecimal(item.Invoice_allocated) != 0)
                    {
                        invoiceAmountTotal = invoiceAmountTotal + Convert.ToDecimal(item.Invoice_amount);
                    }


                    
                    multiplePaymentCheques.Add(item.Cheque_no);


                }
                #endregion
                unallocatedAmountTotal = amountTotal - settledTotal;

                fontStyle = FontFactory.GetFont("Tahoma", 7f, 1);

                pdfPCell = new PdfPCell(new Phrase("Total", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                pdfPCell.Colspan = 10;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(Convert.ToDecimal(amountTotal).ToString("#,##0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(Convert.ToDecimal(unallocatedAmountTotal).ToString("#,##0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(Convert.ToDecimal(invoiceAmountTotal).ToString("#,##0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(Convert.ToDecimal(osBalanceTotal).ToString("#,##0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(Convert.ToDecimal(settledTotal).ToString("#,##0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase("", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.Padding = headerCellPadding;
                pdfPCell.ExtraParagraphSpace = 0;
                main_table.AddCell(pdfPCell);

                main_table.CompleteRow();
            }

        }
       

    }

}

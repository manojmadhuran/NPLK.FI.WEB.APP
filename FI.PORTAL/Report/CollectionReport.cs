using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using FI.PORTAL.dbconnect;
using FI.PORTAL.ViewModels;

namespace FI.PORTAL.Report
{
    public class CollectionReport
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
        #endregion

        public byte[] PrepareReport(List<FullCollectionData_Result> crrd, string username)
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
            this.MainTable();
            this.DetailsTable();
            this.DocTable(username);

            details_table.SpacingBefore = 10f;
            details_table.SpacingAfter = 12.5f;
            main_table.SpacingBefore = 10f;
            main_table.SpacingAfter = 12.5f;

            document.Add(header_table);
            document.Add(subheader_table);

            float[] details_table_widths = new float[] { 8, 8, 8, 10};
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


        private void MainTable()
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



            int same = 0;
            string currentCustomerCode = "none";
            string paymentKey = "none";
            decimal amountTotal = 0;
            decimal unallocatedAmountTotal = 0;
            decimal invoiceAmountTotal = 0;
            decimal osBalanceTotal = 0;
            decimal settledTotal = 0;


            if (data.Count() > 0)
            {
                foreach (var item in data)
                {

                    if (currentCustomerCode.Equals(item.customer_code) && paymentKey.Equals(item.payment_key)) { same = 1; } else { same = 0; }
                    currentCustomerCode = item.customer_code; paymentKey = item.payment_key;

                    invoiceAmountTotal = invoiceAmountTotal + (decimal)item.invoice_amount;
                    osBalanceTotal = osBalanceTotal + (decimal)item.os_balance;
                    settledTotal = settledTotal + (decimal)item.invoice_allocated;

                    if (same == 1)
                    {
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

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_date == null ? "" : item.invoice_date.Value.ToShortDateString(), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_no == null ? "" : item.invoice_no, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.payment_term == null ? "" : item.payment_term, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_amount == null ? "" : Convert.ToDecimal(item.invoice_amount.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.os_balance == null ? "" : Convert.ToDecimal(item.os_balance.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_allocated == null ? "" : Convert.ToDecimal(item.invoice_allocated.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.nod == null ? "" : item.nod.ToString(), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.remarks == null ? "" : item.remarks, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_acknowledge == true ? "Yes" : "No", fontStyle));
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
                    else
                    {

                        amountTotal = amountTotal + (decimal)item.payment_amount;
                        unallocatedAmountTotal = unallocatedAmountTotal + (decimal)item.unallocated_amount;

                        pdfPCell = new PdfPCell(new Phrase(item.customer_code == null ? "" : item.customer_code, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.CusName == null ? "" : item.CusName, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.payment_type == null ? "" : item.payment_type, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.cheque_no == null ? "" : item.cheque_no, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.issued_bank == null ? "" : item.issued_bank, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.issued_branch == null ? "" : item.issued_branch, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.banking_date == null ? "" : item.banking_date.Value.ToShortDateString(), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.deposited_bank == null ? "" : item.deposited_bank, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.deposited_branch == null ? "" : item.deposited_branch, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.receipt_no == null ? "" : item.receipt_no, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.payment_amount == null ? "" : Convert.ToDecimal(item.payment_amount.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.unallocated_amount == null ? "" : Convert.ToDecimal(item.unallocated_amount.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_date == null ? "" : item.invoice_date.Value.ToShortDateString(), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_no == null ? "" : item.invoice_no, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.payment_term == null ? "" : item.payment_term, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_amount == null ? "" : Convert.ToDecimal(item.invoice_amount.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.os_balance == null ? "" : Convert.ToDecimal(item.os_balance.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_allocated == null ? "" : Convert.ToDecimal(item.invoice_allocated.Value).ToString("#,##0.00"), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.nod == null ? "" : item.nod.ToString(), fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.remarks == null ? "" : item.remarks, fontStyle));
                        pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfPCell.BackgroundColor = BaseColor.WHITE;
                        pdfPCell.Padding = headerCellPadding;
                        pdfPCell.ExtraParagraphSpace = 0;
                        main_table.AddCell(pdfPCell);

                        pdfPCell = new PdfPCell(new Phrase(item.invoice_acknowledge == true ? "Yes" : "No", fontStyle));
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
                #endregion

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
using FI.PORTAL.dbconnect;
using FI.PORTAL.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Report
{
    public class CustomerRequestReport
    {
        #region Declaration
        Document document;
        Font fontStyle, fontStyleBold, fontStyleNormal, heading, subheading;
        PdfPTable evaluation_table = new PdfPTable(8);
        PdfPTable main_table = new PdfPTable(2);
        PdfPTable doc_table = new PdfPTable(2);
        PdfPTable owner_table = new PdfPTable(3);
        PdfPTable paintbrands_table = new PdfPTable(3);
        PdfPTable other_companies_table = new PdfPTable(4);
        PdfPTable header_table = new PdfPTable(1);
        PdfPTable subheader_table = new PdfPTable(1);
        PdfPCell pdfPCell;
        int headerCellPadding = 6;
        MemoryStream memoryStream = new MemoryStream();
        CustomerRegistrationRequestData data = new CustomerRegistrationRequestData();
        #endregion

        public byte[] PrepareReport(CustomerRegistrationRequestData crrd)
        {
            data = crrd;

            #region
            document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            document.SetPageSize(PageSize.A4);
            document.SetMargins(40f, 20f, 20f, 20f);
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            #endregion


            this.HeaderTable();
            this.MainTable();

            document.Add(header_table);
            document.Add(main_table);

            document.NewPage();
            this.SubheaderTable("Evaluation History");
            this.EvaluationHistoryTable();
            document.Add(subheader_table);
            document.Add(evaluation_table);

            this.SubheaderTable("Owners/Partners/Directors");
            this.OwnerTable();
            document.Add(subheader_table);
            document.Add(owner_table);

            this.SubheaderTable("Available Main Paint Brands");
            this.PaintBrandsTable();
            document.Add(subheader_table);
            document.Add(paintbrands_table);

            this.SubheaderTable("Credit Offered by Other Companies");
            this.OtherComapniesTable();
            document.Add(subheader_table);
            document.Add(other_companies_table);

            this.SubheaderTable("Attached Documents");
            this.DocTable();
            document.Add(subheader_table);
            document.Add(doc_table);

            document.Close();
            return memoryStream.ToArray();

        }

        

        private void HeaderTable()
        {
            header_table.WidthPercentage = 100;
            header_table.HorizontalAlignment = Element.ALIGN_CENTER;

            heading = FontFactory.GetFont("Tahoma", 16, 1);
            int cellPadding = 15;
            int borderSize = 0;

            #region Table body

            pdfPCell = new PdfPCell(new Phrase("Customer Registration Request Report", heading));
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

        private void EvaluationHistoryTable()
        {
            evaluation_table.WidthPercentage = 100;
            evaluation_table.HorizontalAlignment = Element.ALIGN_LEFT;
            decimal x = 0;

            #region Table header
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            pdfPCell = new PdfPCell(new Phrase("Evaluation ID", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Requested Credit Limit (Rs)", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Requested Credit Period", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Requested Discount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Remarks", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Evaluation By", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Created Date", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Status", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            evaluation_table.AddCell(pdfPCell);


            evaluation_table.CompleteRow();
            #endregion

            #region Table Body

            fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            foreach (var item in data.RequestEvaluations)
            {

                pdfPCell = new PdfPCell(new Phrase(item.EvalID.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                try
                {
                    x = (decimal)item.RecLimit;
                }
                catch (Exception e)
                {

                }

                pdfPCell = new PdfPCell(new Phrase(x.ToString("0.00"), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.RecPeriod.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.RecDiscount.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.Remarks.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.Role.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.CreatedDate.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(item.Status.ToString(), fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                evaluation_table.AddCell(pdfPCell);

                evaluation_table.CompleteRow();
            }
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
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = 20;
            pdfPCell.PaddingTop = 32;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            subheader_table.AddCell(pdfPCell);

            subheader_table.CompleteRow();
            #endregion
        }

        private void MainTable()
        {
            main_table.WidthPercentage = 100;
            main_table.HorizontalAlignment = Element.ALIGN_LEFT;
            main_table.SetWidths(new float[] { 50f, 150f });

            fontStyleBold = FontFactory.GetFont("Tahoma", 10f, 1);
            fontStyleNormal = FontFactory.GetFont("Tahoma", 10f, 0);
            int cellPadding = 8;
            int borderSize = 0;


            #region Table body

            pdfPCell = new PdfPCell(new Phrase("Request ID: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusReqID.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Rep Name: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.REPName.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer Name: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusName.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer Address: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusAddress.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer Telephone: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusTel.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer FAX: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusFax.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer Email: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusEmail.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Factory Address: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DelAddress.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Factory Telephone: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DelTel.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Factory Fax: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DelFax.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Factory Email: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DelEmail.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Sales Area: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.SalesAreaName.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Sales Area Code: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.SalesCode.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("VAT Registration No: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.VatRegNo.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Date of Initial Customer Visit: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DateofInitialCusVisit.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Name of the Person Deal with: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.PersonDealWith.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Designation of the Person Deal with: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.DesignationPersonDealWith.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Contact No. of the Person Deal with: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.ContactNoPersonDealWith.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Nature of the Business: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BusinessNature.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Nature: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.Nature.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("How Long Business Operates: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BusinessDuration.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Area of the Business Premises (SF): ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BusinessArea.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Ownership of the Business Premises: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BusinessPremises.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("SVAT Customer: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.SVAT_Customer ? "Yes" : "No", fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Customer Financial/Wealth Stability: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusFinanceStability.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Customer Payment Strength: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.CusPayStrength.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Initial Order Rs: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.InitialOrder.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("First 6 Months (Avg. Monthly Sale) Rs: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.FirstAvgSales.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Next 6 Months (Avg. Monthly Sale) Rs: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.NextAvgSales.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Estimated Monthly Nippon Paint Purchase Rs: ", fontStyleBold));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.EstiPurchase.ToString(), fontStyleNormal));
            pdfPCell.PaddingBottom = cellPadding;
            pdfPCell.Border = borderSize;
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            main_table.AddCell(pdfPCell);
            main_table.CompleteRow();

            #endregion


        }

        private void OwnerTable()
        {
            owner_table.WidthPercentage = 100;
            owner_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            pdfPCell = new PdfPCell(new Phrase("Name", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            owner_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Contact No.", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            owner_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("ID No.", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            owner_table.AddCell(pdfPCell);

            owner_table.CompleteRow();
            #endregion

            #region Table Body
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            if (data.RequestOwners.Count() == 0)
            {
                pdfPCell = new PdfPCell(new Phrase("No data found!", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                pdfPCell.Colspan = 3;
                owner_table.AddCell(pdfPCell);
                owner_table.CompleteRow();
            }
            else
            {
                foreach (var item in data.RequestOwners)
                {

                    pdfPCell = new PdfPCell(new Phrase(item.Owner_name.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    owner_table.AddCell(pdfPCell);



                    pdfPCell = new PdfPCell(new Phrase(item.Contact_no.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    owner_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.NIC.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    owner_table.AddCell(pdfPCell);
                    owner_table.CompleteRow();
                }

            }
            #endregion
        }

        private void PaintBrandsTable()
        {
            paintbrands_table.WidthPercentage = 100;
            paintbrands_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            pdfPCell = new PdfPCell(new Phrase("Brand", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            paintbrands_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Stock Value (Rs)", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            paintbrands_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Monthly Off Take Value (Rs)", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            paintbrands_table.AddCell(pdfPCell);

            paintbrands_table.CompleteRow();
            #endregion

            #region Table Body
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            if (data.RequestPaintBrands.Count() == 0)
            {
                pdfPCell = new PdfPCell(new Phrase("No data found!", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                pdfPCell.Colspan = 3;
                paintbrands_table.AddCell(pdfPCell);
                paintbrands_table.CompleteRow();
            }
            else
            {
                foreach (var item in data.RequestPaintBrands)
                {
                    pdfPCell = new PdfPCell(new Phrase(item.PaintBrand.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    paintbrands_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.StockVal.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    paintbrands_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.MonthlyTOVal.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    paintbrands_table.AddCell(pdfPCell);

                    paintbrands_table.CompleteRow();
                }

            }
            #endregion
        }

        private void OtherComapniesTable()
        {
            other_companies_table.WidthPercentage = 100;
            other_companies_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header
            fontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            pdfPCell = new PdfPCell(new Phrase("Company", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            other_companies_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Credit Limit (Rs)", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            other_companies_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Credit Period", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            other_companies_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Discount", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            other_companies_table.AddCell(pdfPCell);

            other_companies_table.CompleteRow();
            #endregion

            #region Table Body
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);

            if (data.RequestCompanies.Count() == 0)
            {
                pdfPCell = new PdfPCell(new Phrase("No data found!", fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                pdfPCell.ExtraParagraphSpace = 0;
                pdfPCell.Colspan = 4;
                other_companies_table.AddCell(pdfPCell);
                other_companies_table.CompleteRow();
            }
            else
            {
                foreach (var item in data.RequestCompanies)
                {
                    pdfPCell = new PdfPCell(new Phrase(item.Company.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    other_companies_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.CreditLimit.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    other_companies_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.CreditPeriod.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    other_companies_table.AddCell(pdfPCell);

                    pdfPCell = new PdfPCell(new Phrase(item.Discount.ToString(), fontStyle));
                    pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    pdfPCell.BackgroundColor = BaseColor.WHITE;
                    pdfPCell.ExtraParagraphSpace = 0;
                    other_companies_table.AddCell(pdfPCell);

                    other_companies_table.CompleteRow();
                }

            }
            #endregion
        }

        private void DocTable()
        {
            doc_table.WidthPercentage = 50;
            doc_table.HorizontalAlignment = Element.ALIGN_LEFT;

            #region Table header 
            fontStyle = FontFactory.GetFont("Tahoma",10f, 1);

            pdfPCell = new PdfPCell(new Phrase("Document Type", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Attached or Not", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.Padding = headerCellPadding;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);

            doc_table.CompleteRow();
            #endregion

            #region Table Body
            fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);


            pdfPCell = new PdfPCell(new Phrase("Copy of Business Reg. Certificates", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BusinessRegCert ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Copy of VAT Reg. Certificates", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.VatRegCert ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Copy of ID of Owners of the Business", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.IDofOwner ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Form 20", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.Form20 ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();

            pdfPCell = new PdfPCell(new Phrase("Copies of Last 2 Months Bank Statements", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.BankStatments ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Pictures of Business Place", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.Business_Place ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("Fully Completed Application Form by the Customer", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.Application_Form ? "Yes" : "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();


            pdfPCell = new PdfPCell(new Phrase("SVAT Validation letter issued by IRD", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            pdfPCell = new PdfPCell(new Phrase(data.RequestHeader.SVAT_Validation ? "Yes": "No", fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            doc_table.AddCell(pdfPCell);
            doc_table.CompleteRow();


            #endregion
        }
    }
}
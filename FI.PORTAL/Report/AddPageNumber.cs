using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FI.PORTAL.Report
{
    public class AddPageNumber
    {

        public byte[] PrepareReport(byte[] data)
        {
            Font fontStyle = FontFactory.GetFont("Calibri", 8f, 0);
            MemoryStream pageStream;
            using (MemoryStream stream = new MemoryStream())
            {
                PdfReader reader = new PdfReader(data);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.ALIGN_RIGHT, new Phrase("Page " + i.ToString() + " of " + pages.ToString(), fontStyle), 568f, 15f, 0);
                        //Rectangle cropBox = reader.GetCropBox(1);
                        //Rectangle bottomRight = new Rectangle(cropBox.GetRight(216), cropBox.Bottom, cropBox.Right, cropBox.GetBottom(146));
                        //EmptyTextBoxSimple(stamper, 1, bottomRight, BaseColor.WHITE);
                        //ColumnText columnText = GenerateTextBox(stamper, 1, bottomRight);
                        //columnText.AddText(new Phrase("Some test text to draw into a text box in the lower right corner of the first page"));
                        //columnText.Go();
                    }

                }

                pageStream = stream;
            }

            return pageStream.ToArray();

        }

        ColumnText GenerateTextBox(PdfStamper stamper, int pageNumber, Rectangle boxArea)
        {
            PdfContentByte canvas = stamper.GetOverContent(pageNumber);
            ColumnText columnText = new ColumnText(canvas);
            columnText.SetSimpleColumn(boxArea);
            return columnText;
        }

        void EmptyTextBoxSimple(PdfStamper stamper, int pageNumber, Rectangle boxArea, BaseColor fillColor)
        {
            PdfContentByte canvas = stamper.GetOverContent(pageNumber);
            canvas.SaveState();
            canvas.SetColorFill(fillColor);
            canvas.Rectangle(boxArea.Left, boxArea.Bottom, boxArea.Width, boxArea.Height);
            canvas.Fill();
            canvas.RestoreState();
        }


    }
}
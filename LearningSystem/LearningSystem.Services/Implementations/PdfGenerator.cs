namespace LearningSystem.Services.Implementations
{
    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;
    using System.IO;

    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GeneratePdfFromHtml(string html)
        {

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                using (StringReader stringReader=new StringReader(html))
                {
                    htmlparser.Parse(stringReader);
                }

                pdfDoc.Close();

                return memoryStream.ToArray();
            }
        }
    }
}
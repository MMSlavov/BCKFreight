namespace BCKFreightTMS.Services
{
    using System;
    using System.IO;

    using SelectPdf;
    using TheArtOfDev.HtmlRenderer.PdfSharp;

    public class PdfService : IPdfService
    {
        public byte[] PdfSharpConvert(string html)
        {
            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            return res;
        }

        public byte[] SelectPdfConvert(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MarginBottom = 50;
            converter.Options.MarginTop = 50;
            converter.Options.MarginLeft = 50;
            converter.Options.MarginRight = 50;

            byte[] res = null;
            using (MemoryStream ms = new MemoryStream())
            {
                var pdf = converter.ConvertHtmlString(html);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            return res;
        }

        public void SavePdf(byte[] data, string path)
        {
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

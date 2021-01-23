namespace BCKFreightTMS.Services
{
    using System;
    using System.IO;

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

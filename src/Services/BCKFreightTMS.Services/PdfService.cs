namespace BCKFreightTMS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    using SelectPdf;
    using Tesseract;
    using WIA;

    public class PdfService : IPdfService, IDisposable
    {
        private readonly TesseractEngine ocr;
        private bool disposed;

        public PdfService()
        {
            this.ocr = new TesseractEngine("./tessdata", "bul+eng", EngineMode.LstmOnly);
        }

        public List<Image> Scan()
        {
            return WIAScanner.Scan();
        }

        // public string ScanAndOCR()
        // {
        //    var images = WIAScanner.Scan();
        //    string result = string.Empty;

        // using (var ms = new MemoryStream())
        //    {
        //        images[0].Save(ms, images[0].RawFormat);
        //        result = this.api.RecognizeImage(ms);
        //    }

        // return result;
        // }
        public IEnumerable<string> ScanTessOCR()
        {
            var images = WIAScanner.Scan();
            var result = new List<string>();

            using (var ms = new MemoryStream())
            {
                foreach (var image in images)
                {
                    image.Save(ms, image.RawFormat);
                    var pic = Pix.LoadFromMemory(ms.ToArray()).Deskew();
                    pic.Save("ocrImg.png", ImageFormat.Png);

                    var res = this.ocr.Process(pic);
                    result.Add(res.GetText().Trim());
                }
            }

            return result;
        }

        public string FileTessOCR(byte[] data)
        {
            var pic = Pix.LoadFromMemory(data);
            var res = this.ocr.Process(pic);
            var result = res.GetText();

            return result.Trim();
        }

        public string[] GetScaners()
        {
            var deviceManager = new DeviceManager();
            var scaners = WIAScanner.GetDevices();

            return scaners.ToArray();
        }

        public byte[] SelectPdfConvert(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MarginBottom = 70;
            converter.Options.MarginTop = 40;
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.ocr.Dispose();
            }

            this.disposed = true;
        }

        // public byte[] PdfSharpConvert(string html)
        // {
        //    byte[] res = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
        //        pdf.Save(ms);
        //        res = ms.ToArray();
        //    }

        // return res;
        // }
    }
}

namespace BCKFreightTMS.Services
{
    using System.Collections.Generic;
    using System.Drawing;

    public interface IPdfService
    {
        // public byte[] PdfSharpConvert(string html);
        public byte[] SelectPdfConvert(string html);

        public void SavePdf(byte[] data, string path);

        public string[] GetScaners();

        public List<Image> Scan();

        // public string ScanAndOCR();
        public IEnumerable<string> ScanTessOCR();

        public string FileTessOCR(byte[] data);
    }
}

﻿namespace BCKFreightTMS.Services
{
    public interface IPdfService
    {
        // public byte[] PdfSharpConvert(string html);
        public byte[] SelectPdfConvert(string html);

        public void SavePdf(byte[] data, string path);
    }
}
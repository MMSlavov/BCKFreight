namespace BCKFreightTMS.Services.Data
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Invoices;

    public interface IInvoicesService
    {
        public InvoiceInEditModel LoadInvoiceEditModel(string invoiceId);

        public InvoiceOutEditModel LoadInvoiceOutEditModel(string invoiceId);

        public Task<string> SaveInvoiceIn(InvoiceInEditModel input);

        public Task UpdateInvoiceInStatusAsync(string invoiceId, string status);

        public Task UpdateInvoiceOutStatusAsync(string invoiceId, string status);

        public Task MarkInvoiceInForApproval(string invoiceId);

        public InvoiceOutInputModel LoadInvoiceOutModel(string orderId);

        public Task<string> CreateInvoiceOut(InvoiceOutInputModel input);

        public Task<string> SaveInvoiceOut(InvoiceOutEditModel input);

        public InvoiceModel GenerateInvoiceModel(string invoiceId);

        public Task<string> GenerateInvoiceHtml(string id);

        public Task<byte[]> GenerateInvoicePdf(string id);
    }
}

namespace BCKFreightTMS.Services.Data
{
    using System.Threading.Tasks;

    using BCKFreightTMS.Web.ViewModels.Invoices;

    public interface IInvoicesService
    {
        public InvoiceInEditModel LoadInvoiceEditModel(string invoiceId);

        public InvoiceOutEditModel LoadInvoiceOutEditModel(string invoiceId);

        public Task<string> SaveInvoiceIn(InvoiceInEditModel input);

        public Task UpdateInvoiceInStatus(string invoiceId, string status);

        public Task UpdateInvoiceOutStatus(string invoiceId, string status);

        public Task MarkInvoiceInForApproval(string invoiceId);

        public InvoiceOutInputModel LoadInvoiceOutModel(string orderId);

        public Task<string> SaveInvoiceOut(InvoiceOutInputModel input);
    }
}

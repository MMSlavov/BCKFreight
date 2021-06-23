namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using BCKFreightTMS.Data.Models;
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

        public InvoiceNoteOutInputModel LoadInvoiceNoteOutModel(string invoiceId);

        public Task PayInvoiceIn(string invoiceId);

        public Task PayInvoiceOut(string invoiceId);

        public IEnumerable<ListInvoiceInModel> LoadInvoiceInList(Expression<Func<InvoiceIn, bool>> filter);

        public IEnumerable<ListInvoiceOutModel> LoadInvoiceOutList(Expression<Func<InvoiceOut, bool>> filter);

        public IEnumerable<OrderToInvoiceModel> GetOrderTosInvoicing(string orderId);

        public Task<string> CreateInvoiceOut(InvoiceOutInputModel input);

        public Task<string> CreateInvoiceNoteOut(InvoiceNoteOutInputModel input);

        public Task<string> SaveInvoiceOut(InvoiceOutEditModel input);

        public InvoiceModel GenerateInvoiceModel(string invoiceId);

        public Task<string> GenerateInvoiceHtml(string id);

        public Task<byte[]> GenerateInvoicePdf(string id);
    }
}

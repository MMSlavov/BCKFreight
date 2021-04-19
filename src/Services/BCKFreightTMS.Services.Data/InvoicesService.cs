namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using BCKFreightTMS.Common;
    using BCKFreightTMS.Common.Enums;
    using BCKFreightTMS.Data.Common.Repositories;
    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Web.ViewModels.Invoices;
    using BCKFreightTMS.Web.ViewModels.Orders;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class InvoicesService : IInvoicesService
    {
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<InvoiceStatus> invoiceStatuses;
        private readonly IDeletableEntityRepository<OrderTo> orderTos;
        private readonly IDeletableEntityRepository<VATReason> vatReasons;
        private readonly IPdfService pdfService;
        private readonly IViewRenderService viewRenderService;
        private readonly IDeletableEntityRepository<InvoiceOut> invoiceOuts;
        private IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private IOrdersService ordersService;

        public InvoicesService(
                IDeletableEntityRepository<InvoiceIn> invoiceIns,
                IDeletableEntityRepository<InvoiceOut> invoiceOuts,
                IDeletableEntityRepository<InvoiceStatus> invoiceStatuses,
                IDeletableEntityRepository<OrderTo> orderTos,
                IDeletableEntityRepository<VATReason> vatReasons,
                IPdfService pdfService,
                IViewRenderService viewRenderService,
                IOrdersService ordersService,
                IMapper mapper)
        {
            this.invoiceIns = invoiceIns;
            this.invoiceOuts = invoiceOuts;
            this.invoiceStatuses = invoiceStatuses;
            this.orderTos = orderTos;
            this.vatReasons = vatReasons;
            this.pdfService = pdfService;
            this.viewRenderService = viewRenderService;
            this.ordersService = ordersService;
            this.mapper = mapper;
        }

        public InvoiceInEditModel LoadInvoiceEditModel(string invoiceId)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(o => o.Id == invoiceId);

            var invoiceInModel = this.mapper.Map<InvoiceInEditModel>(invoice);
            var orderTo = invoice.OrderTos.First();
            invoiceInModel.BankDetailsItems = orderTo.CarrierOrder.Company.BankDetails
                                                     .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceInModel.CreatorCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.Order.Creator.Company);
            invoiceInModel.CarrierCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.CarrierOrder.Company);
            foreach (var orderToInvoice in invoiceInModel.OrderTos)
            {
                orderToInvoice.IsDocValid = true;
                if (orderToInvoice.Documentation.CMR != orderToInvoice.DocumentationRecievedDocumentation.CMR ||
                    orderToInvoice.Documentation.AOA != orderToInvoice.DocumentationRecievedDocumentation.AOA ||
                    orderToInvoice.Documentation.BillOfLading != orderToInvoice.DocumentationRecievedDocumentation.BillOfLading ||
                    orderToInvoice.Documentation.DeliveryNote != orderToInvoice.DocumentationRecievedDocumentation.DeliveryNote ||
                    orderToInvoice.Documentation.ListItems != orderToInvoice.DocumentationRecievedDocumentation.ListItems ||
                    orderToInvoice.Documentation.PackingList != orderToInvoice.DocumentationRecievedDocumentation.PackingList ||
                    orderToInvoice.Documentation.Invoice != orderToInvoice.DocumentationRecievedDocumentation.Invoice ||
                    orderToInvoice.Documentation.BillOfGoods != orderToInvoice.DocumentationRecievedDocumentation.BillOfGoods ||
                    orderToInvoice.Documentation.WeighingNote != orderToInvoice.DocumentationRecievedDocumentation.WeighingNote)
                {
                    orderToInvoice.IsDocValid = false;
                }
            }

            invoiceInModel.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();
            VATReasonsIn res;
            invoiceInModel.ReasonNoVATItems = this.vatReasons.AllAsNoTracking()
                                                           .Select(r => new KeyValuePair<string, string>(r.Id.ToString(), r.Name))
                                                           .ToList()
                                                           .Where(r => Enum.TryParse<VATReasonsIn>(r.Value, out res));

            // var receivedDoc = order.Documentation.RecievedDocumentation;
            // model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return invoiceInModel;
        }

        public InvoiceOutEditModel LoadInvoiceOutEditModel(string invoiceId)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(o => o.Id == invoiceId);

            var invoiceOutModel = this.mapper.Map<InvoiceOutEditModel>(invoice);
            var orderTo = invoice.OrderTos.First();
            invoiceOutModel.BankDetailsItems = orderTo.Order.Creator.Company.BankDetails
                                                     .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceOutModel.CreatorCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.Order.Creator.Company);
            invoiceOutModel.ClientCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.Order.OrderFrom.Company);

            invoiceOutModel.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();
            VATReasonsOut res;
            invoiceOutModel.ReasonNoVATItems = this.vatReasons.AllAsNoTracking()
                                                           .Select(r => new KeyValuePair<string, string>(r.Id.ToString(), r.Name))
                                                           .ToList()
                                                           .Where(r => Enum.TryParse<VATReasonsOut>(r.Value, out res));

            return invoiceOutModel;
        }

        public async Task<string> SaveInvoiceIn(InvoiceInEditModel input)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(i => i.Id == input.Id);

            invoice.Number = input.Number;
            invoice.CreateDate = input.CreateDate;
            invoice.ReceiveDate = input.ReceiveDate;
            invoice.DueDays = input.DueDays;
            invoice.VATReasonId = input.VATReasonId;
            foreach (var orderTo in input.OrderTos)
            {
                var recDocInput = orderTo.DocumentationRecievedDocumentation;
                var recDoc = invoice.OrderTos.FirstOrDefault(o => o.Id == orderTo.Id).Documentation.RecievedDocumentation;
                recDoc = this.mapper.Map<DocumentationInputModel, Documentation>(recDocInput, recDoc);
            }

            foreach (var orderTo in invoice.OrderTos)
            {
                if (!input.OrderTos.Any(o => o.Id == orderTo.Id))
                {
                    orderTo.InvoiceInId = null;
                }
            }

            this.invoiceIns.Update(invoice);
            await this.invoiceIns.SaveChangesAsync();

            return invoice.Id;
        }

        public async Task<string> CreateInvoiceOut(InvoiceOutInputModel input)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(i => i.Id == input.InvoiceOut.Id);
            if (invoice is null)
            {
                invoice = new InvoiceOut();

                await this.invoiceOuts.AddAsync(invoice);
            }

            invoice.Number = input.InvoiceOut.Number;
            invoice.CreateDate = input.InvoiceOut.CreateDate;
            invoice.DueDays = input.InvoiceOut.DueDays;
            invoice.BankDetailsId = input.InvoiceOut.BankDetailsId;
            invoice.VATReasonId = input.InvoiceOut.ReasonNoVATId;
            foreach (var orderToInput in input.OrderTos)
            {
                var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderToInput.Id);
                if (orderTo.InvoiceOut is null)
                {
                    orderTo.InvoiceOut = invoice;
                }
            }

            await this.orderTos.SaveChangesAsync();

            foreach (var orderTo in invoice.OrderTos)
            {
                if (!input.OrderTos.Any(o => o.Id == orderTo.Id))
                {
                    orderTo.InvoiceInId = null;
                }
            }

            this.invoiceOuts.Update(invoice);
            await this.invoiceOuts.SaveChangesAsync();

            return invoice.Id;
        }

        public async Task<string> SaveInvoiceOut(InvoiceOutEditModel input)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(i => i.Id == input.Id);
            if (invoice is null)
            {
                throw new ArgumentException("Invoice do not exist.");
            }

            invoice.Number = input.Number;
            invoice.CreateDate = input.CreateDate;
            invoice.DueDays = input.DueDays;
            invoice.BankDetailsId = input.BankDetailsId;
            invoice.VATReasonId = input.VATReasonId;
            foreach (var orderToInput in input.OrderTos)
            {
                var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderToInput.Id);
                if (orderTo.InvoiceOut is null)
                {
                    orderTo.InvoiceOut = invoice;
                }
            }

            await this.orderTos.SaveChangesAsync();

            foreach (var orderTo in invoice.OrderTos)
            {
                if (!input.OrderTos.Any(o => o.Id == orderTo.Id))
                {
                    orderTo.InvoiceInId = null;
                }
            }

            this.invoiceOuts.Update(invoice);
            await this.invoiceOuts.SaveChangesAsync();

            return invoice.Id;
        }

        public async Task MarkInvoiceInForApproval(string invoiceId)
        {
            await this.UpdateInvoiceInStatusAsync(invoiceId, OrderStatusNames.AwaitingApproval.ToString());
        }

        public async Task UpdateInvoiceInStatusAsync(string invoiceId, string status)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(o => o.Id == invoiceId);
            invoice.Status = this.invoiceStatuses.All()
                                                 .FirstOrDefault(s => s.Name == status);
            await this.invoiceIns.SaveChangesAsync();
        }

        public async Task UpdateInvoiceOutStatusAsync(string invoiceId, string status)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(o => o.Id == invoiceId);
            invoice.Status = this.invoiceStatuses.All()
                                                 .FirstOrDefault(s => s.Name == status);
            await this.invoiceOuts.SaveChangesAsync();
        }

        public InvoiceOutInputModel LoadInvoiceOutModel(string orderId)
        {
            var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderId);

            var invoiceOutModel = this.mapper.Map<InvoiceOutInputModel>(orderTo);
            invoiceOutModel.InvoiceOut = new InvoiceOutModel();
            invoiceOutModel.InvoiceOut.Number = this.GenerateInvoiceOutNumber();
            invoiceOutModel.InvoiceOut.BankDetailsItems = orderTo.Order.Creator.Company.BankDetails
                                                               .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceOutModel.InvoiceOut.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();
            VATReasonsOut res;
            invoiceOutModel.InvoiceOut.ReasonNoVATItems = this.vatReasons.AllAsNoTracking()
                                                           .Select(r => new KeyValuePair<string, string>(r.Id.ToString(), r.Name))
                                                           .ToList()
                                                           .Where(r => Enum.TryParse<VATReasonsOut>(r.Value, out res));

            invoiceOutModel.OrderTos = new List<OrderToInvoiceModel>();
            invoiceOutModel.OrderTos.Add(this.mapper.Map<OrderToInvoiceModel>(orderTo));

            if (invoiceOutModel.OrderCreatorCompany.TaxCountryName == TaxCountryNames.Румъния.ToString())
            {
                invoiceOutModel.SelectedReasonId = invoiceOutModel.InvoiceOut.ReasonNoVATItems.FirstOrDefault(r => r.Value == VATReasonsOut.Чл82ал2.ToString()).Key;
            }
            else
            {
                if (invoiceOutModel.OrderTos.Any(ot => ot.NoVAT))
                {
                    invoiceOutModel.SelectedReasonId = invoiceOutModel.InvoiceOut.ReasonNoVATItems.FirstOrDefault(r => r.Value == VATReasonsOut.Чл22ал2.ToString()).Key;
                }
                else
                {
                    invoiceOutModel.SelectedReasonId = invoiceOutModel.InvoiceOut.ReasonNoVATItems.FirstOrDefault(r => r.Value == VATReasonsOut.Чл66ал1.ToString()).Key;
                }
            }

            // var receivedDoc = order.Documentation.RecievedDocumentation;
            // model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return invoiceOutModel;
        }

        public InvoiceModel GenerateInvoiceModel(string invoiceId)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(o => o.Id == invoiceId);
            if (invoice is null)
            {
                throw new ArgumentException("Invoice do not exist.");
            }

            var model = this.mapper.Map<InvoiceModel>(invoice);
            var clientCompany = invoice.OrderTos.First().Order.OrderFrom.Company;
            var creatorCompany = invoice.OrderTos.First().Order.Creator.Company;
            model.ClientCompany = this.mapper.Map<InvoiceCompanyModel>(clientCompany);
            model.CreatorCompany = this.mapper.Map<InvoiceCompanyModel>(creatorCompany);

            return model;
        }

        public async Task<string> GenerateInvoiceHtml(string invoiceId)
        {
            var model = this.GenerateInvoiceModel(invoiceId);
            return await this.viewRenderService.RenderToStringAsync("Invoices/Invoice", model);
        }

        public async Task<byte[]> GenerateInvoicePdf(string invoiceId)
        {
            var model = this.GenerateInvoiceModel(invoiceId);
            var html = await this.viewRenderService.RenderToStringAsync("Invoices/Invoice", model);
            return this.pdfService.SelectPdfConvert(html);
        }

        public Task ApproveInvoice(InvoiceInInputModel input)
        {
            throw new NotImplementedException();
        }

        private string GenerateInvoiceOutNumber()
        {
            var ordersCount = this.invoiceOuts.AllAsNoTracking()
                                         .Count() + 1;
            var number = ordersCount.ToString().PadLeft(8, '0');

            return string.Format(GlobalConstants.InvoiceOutNumberFormat, number);
        }
    }
}

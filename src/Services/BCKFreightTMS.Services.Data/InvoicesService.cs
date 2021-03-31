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
        private readonly IDeletableEntityRepository<InvoiceOut> invoiceOuts;
        private IDeletableEntityRepository<InvoiceIn> invoiceIns;
        private IOrdersService ordersService;

        public InvoicesService(
                IDeletableEntityRepository<InvoiceIn> invoiceIns,
                IDeletableEntityRepository<InvoiceOut> invoiceOuts,
                IDeletableEntityRepository<InvoiceStatus> invoiceStatuses,
                IDeletableEntityRepository<OrderTo> orderTos,
                IOrdersService ordersService,
                IMapper mapper)
        {
            this.invoiceIns = invoiceIns;
            this.invoiceOuts = invoiceOuts;
            this.invoiceStatuses = invoiceStatuses;
            this.orderTos = orderTos;
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

            // var receivedDoc = order.Documentation.RecievedDocumentation;
            // model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return invoiceInModel;
        }

        public InvoiceOutEditModel LoadInvoiceOutEditModel(string invoiceId)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(o => o.Id == invoiceId);

            var invoiceInModel = this.mapper.Map<InvoiceOutEditModel>(invoice);
            var orderTo = invoice.OrderTos.First();
            invoiceInModel.BankDetailsItems = orderTo.CarrierOrder.Company.BankDetails
                                                     .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceInModel.CreatorCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.Order.Creator.Company);
            invoiceInModel.ClientCompany = this.mapper.Map<InvoiceCompanyModel>(orderTo.Order.OrderFrom.Company);

            invoiceInModel.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();

            return invoiceInModel;
        }

        public async Task<string> SaveInvoiceIn(InvoiceInEditModel input)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(i => i.Id == input.Id);

            invoice.Number = input.Number;
            invoice.CreateDate = input.CreateDate;
            invoice.ReceiveDate = input.ReceiveDate;
            invoice.DueDays = input.DueDays;
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

        public async Task<string> SaveInvoiceOut(InvoiceOutInputModel input)
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
            await this.UpdateInvoiceInStatus(invoiceId, OrderStatusNames.AwaitingApproval.ToString());
        }

        public async Task UpdateInvoiceInStatus(string invoiceId, string status)
        {
            var invoice = this.invoiceIns.All().FirstOrDefault(o => o.Id == invoiceId);
            invoice.Status = this.invoiceStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == status);
            this.invoiceIns.Update(invoice);
            await this.invoiceIns.SaveChangesAsync();
        }

        public async Task UpdateInvoiceOutStatus(string invoiceId, string status)
        {
            var invoice = this.invoiceOuts.All().FirstOrDefault(o => o.Id == invoiceId);
            invoice.Status = this.invoiceStatuses.AllAsNoTracking()
                                               .FirstOrDefault(s => s.Name == status);
            this.invoiceOuts.Update(invoice);
            await this.invoiceOuts.SaveChangesAsync();
        }

        public InvoiceOutInputModel LoadInvoiceOutModel(string orderId)
        {
            var orderTo = this.orderTos.All().FirstOrDefault(o => o.Id == orderId);

            var invoiceOutModel = this.mapper.Map<InvoiceOutInputModel>(orderTo);
            invoiceOutModel.InvoiceOut = new InvoiceOutModel();
            invoiceOutModel.InvoiceOut.Number = this.GenerateInvoiceOutNumber();
            invoiceOutModel.InvoiceOut.BankDetailsItems = orderTo.Order.OrderFrom.Company.BankDetails
                                                               .Select(bd => new SelectListItem { Value = bd.Id.ToString(), Text = bd.BankIban });
            invoiceOutModel.InvoiceOut.PaymentMethodItems = Enum.GetValues(typeof(PaymentMethods)).Cast<PaymentMethods>().Select(
                            enu => new SelectListItem() { Text = enu.ToString(), Value = enu.ToString() }).ToList();
            invoiceOutModel.OrderTos = new List<OrderToInvoiceModel>();
            invoiceOutModel.OrderTos.Add(this.mapper.Map<OrderToInvoiceModel>(orderTo));

            // var receivedDoc = order.Documentation.RecievedDocumentation;
            // model.RecievedDocumentation = this.mapper.Map<DocumentationInputModel>(receivedDoc);
            return invoiceOutModel;
        }

        public Task ApproveInvoice(InvoiceInInputModel input)
        {
            throw new NotImplementedException();
        }

        private string GenerateInvoiceOutNumber()
        {
            var ordersCount = this.invoiceOuts.AllAsNoTracking()
                                         .Count() + 1;
            var number = ordersCount.ToString().PadLeft(9, '0');

            return string.Format(GlobalConstants.InvoiceOutNumberFormat, number);
        }
    }
}

namespace BCKFreightTMS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using BCKFreightTMS.Web.ViewModels.Orders;

    public class TemplatePlaceholderService : ITemplatePlaceholderService
    {
        public string ReplacePlaceholders(string template, CarrierOrderApplicationModel model)
        {
            if (string.IsNullOrEmpty(template))
            {
                return string.Empty;
            }

            var placeholders = this.BuildPlaceholderDictionary(model);
            
            // Replace all {{PlaceholderName}} with actual values
            var result = template;
            foreach (var placeholder in placeholders)
            {
                var pattern = $"{{{{{placeholder.Key}}}}}";
                result = result.Replace(pattern, placeholder.Value ?? string.Empty);
            }

            // Handle loops for OrderTos
            result = this.ProcessOrderTosLoop(result, model);

            return result;
        }

        public Dictionary<string, string> GetAvailablePlaceholders()
        {
            return new Dictionary<string, string>
            {
                // Header placeholders
                { "{{CompanyName}}", "Order creator company name" },
                { "{{CompanyAddress}}", "Order creator company address" },
                { "{{CompanyCity}}", "Order creator company city" },
                { "{{CompanyCountry}}", "Order creator company country" },
                { "{{CompanyTaxNumber}}", "Order creator company tax number" },
                { "{{CompanyMOL}}", "Order creator company MOL" },
                { "{{CompanyPhone}}", "Order creator company phone" },
                
                // Carrier/Executor placeholders
                { "{{CarrierName}}", "Carrier company name" },
                { "{{CarrierAddress}}", "Carrier company address" },
                { "{{CarrierTaxNumber}}", "Carrier company tax number" },
                { "{{CarrierMOL}}", "Carrier company MOL" },
                { "{{CarrierPhone}}", "Carrier company phone" },
                
                // Order placeholders
                { "{{OrderNumber}}", "Order reference number" },
                { "{{OrderDate}}", "Order date" },
                { "{{DueDays}}", "Payment due days" },
                { "{{NoVAT}}", "VAT status" },
                
                // Loops
                { "{{#OrderTos}}", "Start loop for order destinations" },
                { "{{/OrderTos}}", "End loop for order destinations" },
                { "{{VehicleRegNumber}}", "Vehicle registration number (in loop)" },
                { "{{VehicleTrailerRegNumber}}", "Trailer registration number (in loop)" },
                { "{{CargoName}}", "Cargo name (in loop)" },
                { "{{CargoWeight}}", "Cargo weight (in loop)" },
                { "{{CargoDetails}}", "Cargo details (in loop)" },
                { "{{Price}}", "Price (in loop)" },
                { "{{Currency}}", "Currency (in loop)" },
                
                // Actions loop (nested in OrderTos)
                { "{{#Actions}}", "Start loop for actions (in OrderTos loop)" },
                { "{{/Actions}}", "End loop for actions (in OrderTos loop)" },
                { "{{ActionType}}", "Action type (in Actions loop)" },
                { "{{ActionAddress}}", "Action address (in Actions loop)" },
                { "{{ActionDate}}", "Action date (in Actions loop)" },
                { "{{ActionDetails}}", "Action details (in Actions loop)" },
                
                // Documentation loop (in OrderTos)
                { "{{#Documentation}}", "Start documentation checks (in OrderTos loop)" },
                { "{{/Documentation}}", "End documentation checks" },
                { "{{CMR}}", "CMR document (boolean)" },
                { "{{BillOfLading}}", "Bill of Lading (boolean)" },
                { "{{DeliveryNote}}", "Delivery Note (boolean)" },
                { "{{Invoice}}", "Invoice (boolean)" },
            };
        }

        private Dictionary<string, string> BuildPlaceholderDictionary(CarrierOrderApplicationModel model)
        {
            return new Dictionary<string, string>
            {
                // Company (Order Creator)
                { "CompanyName", model.OrderCreatorCompany?.Name },
                { "CompanyAddress", model.OrderCreatorCompany?.AddressAddressStreetLine },
                { "CompanyCity", model.OrderCreatorCompany?.AddressAddressCity },
                { "CompanyCountry", model.OrderCreatorCompany?.TaxCountryName },
                { "CompanyTaxNumber", model.OrderCreatorCompany?.TaxNumber },
                { "CompanyMOL", $"{model.OrderCreatorCompany?.AddressMOLFirstName} {model.OrderCreatorCompany?.AddressMOLLastName}" },
                { "CompanyPhone", model.OrderCreatorCompany?.ComunicatorsMobile1 },
                
                // Carrier
                { "CarrierName", model.Company?.Name },
                { "CarrierAddress", model.Company?.AddressAddressStreetLine },
                { "CarrierTaxNumber", $"{(model.Company?.TaxNumber?.Length <= 10 ? "BG" : "")}{model.Company?.TaxNumber}" },
                { "CarrierMOL", $"{model.Company?.AddressMOLFirstName} {model.Company?.AddressMOLLastName}" },
                { "CarrierPhone", model.Company?.ComunicatorsMobile1 },
                
                // Order
                { "OrderNumber", model.ReferenceNum },
                { "OrderDate", DateTime.Now.ToString("dd.MM.yyyy'г.'") },
                { "DueDays", model.OrderDueDaysTo.ToString() },
                { "NoVAT", model.NoVAT ? "" : "+ ДДС" },
            };
        }

        private string ProcessOrderTosLoop(string template, CarrierOrderApplicationModel model)
        {
            // Find the OrderTos loop
            var orderTosPattern = @"{{#OrderTos}}(.*?){{/OrderTos}}";
            var orderTosMatch = Regex.Match(template, orderTosPattern, RegexOptions.Singleline);

            if (!orderTosMatch.Success || model.OrderTos == null)
            {
                return template;
            }

            var loopTemplate = orderTosMatch.Groups[1].Value;
            var result = new StringBuilder();

            foreach (var orderTo in model.OrderTos)
            {
                var orderToHtml = loopTemplate;

                // Replace OrderTo placeholders
                orderToHtml = orderToHtml.Replace("{{VehicleRegNumber}}", orderTo.VehicleRegNumber ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{VehicleTrailerRegNumber}}", orderTo.VehicleTrailerRegNumber ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{CargoName}}", orderTo.Cargo?.Name ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{CargoWeight}}", orderTo.Cargo?.WeightGross.ToString() ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{CargoDetails}}", orderTo.Cargo?.Details ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{Price}}", orderTo.PriceNetOut.ToString() ?? string.Empty);
                orderToHtml = orderToHtml.Replace("{{Currency}}", orderTo.CurrencyName ?? string.Empty);

                // Process Actions loop
                orderToHtml = this.ProcessActionsLoop(orderToHtml, orderTo);

                // Process Documentation
                orderToHtml = this.ProcessDocumentation(orderToHtml, orderTo);

                result.Append(orderToHtml);
            }

            return template.Replace(orderTosMatch.Value, result.ToString());
        }

        private string ProcessActionsLoop(string template, OrderToApplicationModel orderTo)
        {
            var actionsPattern = @"{{#Actions}}(.*?){{/Actions}}";
            var actionsMatch = Regex.Match(template, actionsPattern, RegexOptions.Singleline);

            if (!actionsMatch.Success || orderTo.OrderActions == null)
            {
                return template;
            }

            var loopTemplate = actionsMatch.Groups[1].Value;
            var result = new StringBuilder();

            foreach (var action in orderTo.OrderActions)
            {
                var actionHtml = loopTemplate;
                
                actionHtml = actionHtml.Replace("{{ActionType}}", action.TypeName ?? string.Empty);
                actionHtml = actionHtml.Replace("{{ActionAddress}}", this.FormatAddress(action.Address));
                actionHtml = actionHtml.Replace("{{ActionDate}}", action.Until.ToLocalTime().ToShortDateString());
                actionHtml = actionHtml.Replace("{{ActionDetails}}", action.Details ?? string.Empty);

                result.Append(actionHtml);
            }

            return template.Replace(actionsMatch.Value, result.ToString());
        }

        private string ProcessDocumentation(string template, dynamic orderTo)
        {
            if (orderTo.Documentation == null)
            {
                return template;
            }

            var doc = orderTo.Documentation;
            template = template.Replace("{{CMR}}", doc.CMR ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{BillOfLading}}", doc.BillOfLading ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{AOA}}", doc.AOA ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{DeliveryNote}}", doc.DeliveryNote ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{PackingList}}", doc.PackingList ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{ListItems}}", doc.ListItems ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{Invoice}}", doc.Invoice ? "<i class='fas fa-check'></i>" : string.Empty);
            template = template.Replace("{{BillOfGoods}}", doc.BillOfGoods ? "<i class='fas fa-check'></i>" : string.Empty);

            return template;
        }

        private string FormatAddress(dynamic address)
        {
            if (address == null)
            {
                return string.Empty;
            }

            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(address.City))
            {
                parts.Add(address.City);
            }

            if (!string.IsNullOrEmpty(address.Area) && address.Area != address.City)
            {
                parts.Add(address.Area);
            }

            if (!string.IsNullOrEmpty(address.Postcode))
            {
                parts.Add(address.Postcode);
            }

            if (!string.IsNullOrEmpty(address.StreetLine))
            {
                parts.Add(address.StreetLine);
            }

            return string.Join(", ", parts);
        }
    }
}

﻿namespace BCKFreightTMS.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BCKFreightTMS.Data.Models;
    using BCKFreightTMS.Services.Mapping;
    using BCKFreightTMS.Web.ViewModels.Cargos;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OrderToModel : IMapFrom<OrderTo>
    {
        public OrderToModel()
        {
            this.Cargo = new CargoInputModel();
            this.Documentation = new DocumentationInputModel();
        }

        public string Id { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal PriceNetIn { get; set; }

        public int CurrencyInId { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal PriceNetOut { get; set; }

        public int? CurrencyOutId { get; set; }

        public string CarrierOrderCompanyId { get; set; }

        public string VehicleId { get; set; }

        public string VehicleRegNumber { get; set; }

        public string VehicleTrailerId { get; set; }

        public string VehicleTrailerRegNumber { get; set; }

        public string CarrierOrderCompanyComunicatorsMobile1 { get; set; }

        public string ContactComunicatorsMobile1 { get; set; }

        public string ContactLastName { get; set; }

        public string ContactFirstName { get; set; }

        public string CarrierOrderCompanyName { get; set; }

        public string DriverId { get; set; }

        public string ContactId { get; set; }

        public bool IsFinished { get; set; }

        public CargoInputModel Cargo { get; set; }

        public DocumentationInputModel Documentation { get; set; }

        public DocumentationInputModel DocumentationRecievedDocumentation { get; set; }

        public List<ActionCreateInputModel> OrderActions { get; set; }

        public List<ActionStatusInputModel> OrderStatusActions { get; set; }

        public IEnumerable<SelectListItem> ContactItems { get; set; }

        public IEnumerable<SelectListItem> DriverItems { get; set; }

        public IEnumerable<SelectListItem> VehicleItems { get; set; }
    }
}

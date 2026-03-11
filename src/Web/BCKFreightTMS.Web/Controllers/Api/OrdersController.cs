namespace BCKFreightTMS.Web.Controllers.Api
{
    using System;

    using BCKFreightTMS.Services.Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    [IgnoreAntiforgeryToken]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService ordersService;
        private readonly IVehiclesService vehiclesService;

        public OrdersController(IOrdersService ordersService, IVehiclesService vehiclesService)
        {
            this.ordersService = ordersService;
            this.vehiclesService = vehiclesService;
        }

        /// <summary>
        /// Get drivers by company ID
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>List of drivers</returns>
        [HttpGet("drivers/{companyId}")]
        public IActionResult GetDrivers(string companyId)
        {
            try
            {
                var drivers = this.vehiclesService.GetDrivers(companyId);
                return this.Ok(drivers);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get vehicles by company ID
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>List of vehicles</returns>
        [HttpGet("vehicles/{companyId}")]
        public IActionResult GetVehicles(string companyId)
        {
            try
            {
                var vehicles = this.vehiclesService.GetVehicles(companyId);
                return this.Ok(vehicles);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get trailers by company ID
        /// </summary>
        /// <param name="companyId">The company identifier</param>
        /// <returns>List of trailers</returns>
        [HttpGet("trailers/{companyId}")]
        public IActionResult GetTrailers(string companyId)
        {
            try
            {
                var trailers = this.vehiclesService.GetTrailers(companyId);
                return this.Ok(trailers);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get carriers by area
        /// </summary>
        /// <param name="area">The area identifier</param>
        /// <returns>List of carriers</returns>
        [HttpGet("carriers/area/{area}")]
        public IActionResult GetCarriersByArea(string area)
        {
            try
            {
                var carriers = this.ordersService.GetCarriersByArea(area);
                return this.Ok(carriers);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { error = ex.Message });
            }
        }
    }
}

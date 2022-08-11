using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Insurance.Api.Models;
using Insurance.Api.Dtos;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        //private const string ProductApi = "http://localhost:5002";
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        //public HomeController(){}

        /// <summary>
        /// Calculate Insurance
        /// </summary>
        /// <returns>insurance</returns>
        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<IActionResult> CalculateInsurance([FromBody] InsuranceDto toInsure)
        {

            var ProductApi = _config.GetValue<string>("ProductApi");
            BusinessRules.GetProductType(ProductApi, ref toInsure);
            
            if (!toInsure.ProductTypeHasInsurance)
                return Ok(toInsure.InsuranceValue);

            BusinessRules.GetSalesPrice(ProductApi, ref toInsure);

            List<string> typeList = new List<string> {
                    StaticDataProvider.Laptops,
                    StaticDataProvider.DigitalCameras,
                    StaticDataProvider.SmartPhones};

            BusinessRules.CalculateInsuranceValue(ref toInsure, typeList);
            return Ok(toInsure.InsuranceValue);


        }

        /// <summary>
        /// Calculate Insurance
        /// </summary>
        /// <returns>insurance</returns>
        [HttpPost]
        [Route("api/insurance/productOrder")]
        public async Task<IActionResult> CalculateInsurance([FromBody] List<InsuranceDto> toInsure)
        {
            float insurance = 0f;
            var ProductApi = _config.GetValue<string>("ProductApi");

            List<string> typeList = new List<string> {
                    StaticDataProvider.Laptops,
                    StaticDataProvider.SmartPhones};


            toInsure.ForEach(x =>
            {
                BusinessRules.GetProductType(ProductApi, ref x);

                if (x.ProductTypeHasInsurance)
                {
                    BusinessRules.GetSalesPrice(ProductApi, ref x);
                    BusinessRules.CalculateInsuranceValue(ref x, typeList);
                }

                insurance += x.InsuranceValue;
            });

            if (toInsure.Any(x => x.ProductTypeName.Equals(StaticDataProvider.DigitalCameras)))
                insurance += StaticDataProvider.DigitalCamerasAdditionalInsuranceValue;

            return Ok(insurance);

        }

        /// <summary>
        /// Add Surcharge
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/insurance/surcharge")]
        public async Task<IActionResult> AddSurcharge([FromBody] InsuranceDto toInsure)
        {

            var ProductApi = _config.GetValue<string>("ProductApi");
            if (toInsure.SurchargeRate > 100 || toInsure.SurchargeRate < -100)
                return BadRequest("Surcharge Rate is not valid");

            BusinessRules.GetProductType(ProductApi, ref toInsure);
            BusinessRules.AddSurcharge(toInsure);


            return Ok("Surcharge Applied");

            //Log.Error($"There was an error while adding the Surcharge rate\n{ex.Message}");
            //return new ApiResponseModel(ApiResponseState.Error, "There was an error while adding the Surcharge rate", ApiErrorCodeEnum.ProductNotFound);

        }
    }
}
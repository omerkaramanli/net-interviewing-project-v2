using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using Serilog;
using Insurance.Api.Models;
using Insurance.Api.Dtos;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost]
        [Route("api/insurance/product")]
        public ApiResponseModel CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            try
            {
                var ProductApi = _config.GetValue<string>("ProductApi");
                float insurance = 0f;
                //Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} started.");
                BusinessRules.GetProductType(ProductApi, ref toInsure);

                if (!toInsure.ProductTypeHasInsurance)
                    return new ApiResponseModel(ApiResponseState.Success, toInsure.InsuranceValue);

                BusinessRules.GetSalesPrice(ProductApi, ref toInsure);

                List<string> typeList = new List<string> { "Laptops", "Smartphones", "Digital cameras" };
                BusinessRules.CalculateInsuranceValue(ref toInsure, typeList);
                insurance = toInsure.InsuranceValue * (1 + toInsure.SurchargeRate / 100);
                //Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} completed.");
                return new ApiResponseModel(ApiResponseState.Success, toInsure.InsuranceValue);

            }
            catch (Exception ex)
            {
                Log.Error($"Insurance value of {{ProductId = {toInsure.ProductId}}} could not be calculated\n{ex.Message}");
                return new ApiResponseModel(ApiResponseState.Error, ex.Message, ApiErrorCodeEnum.ProductNotFound);

            }
        }

        [HttpPost]
        [Route("api/insurance/productOrder")]
        public ApiResponseModel CalculateInsurance([FromBody] List<InsuranceDto> toInsure)
        {
            float insurance = 0f;

            try
            {
                //               Log.Information($"Calculation of insurance for an order started. Count of products in the order: {toInsure.Count}");

                List<string> typeList = new List<string> { "Laptops", "Smartphones" };
                var ProductApi = _config.GetValue<string>("ProductApi");


                toInsure.ForEach(x =>
                {
                    BusinessRules.GetProductType(ProductApi, ref x);

                    if (x.ProductTypeHasInsurance)
                    {
                        BusinessRules.GetSalesPrice(ProductApi, ref x);
                        BusinessRules.CalculateInsuranceValue(ref x, typeList);
                    }

                    insurance += x.InsuranceValue * (1 + x.SurchargeRate / 100);
                });

                if (toInsure.Any(x => x.ProductTypeName == "Digital cameras")) insurance += 500;

                //                Log.Information($"Calculation of insurance for an order completed.");
                return new ApiResponseModel(ApiResponseState.Success, insurance);

            }
            catch (Exception ex)
            {
                Log.Error($"Insurance of one or more items could not be calculated \n{ex.Message}");
                return new ApiResponseModel(ApiResponseState.Error, ex.Message, ApiErrorCodeEnum.ProductNotFound);
            }
        }

        [HttpPost]
        [Route("api/insurance/surcharge")]
        public void AddSurcharge([FromBody] InsuranceDto toInsure, float surchargeRate)
        {
            toInsure.SurchargeRate = surchargeRate;
        }

        //private const string ProductApi = "http://localhost:5002";
    }
}
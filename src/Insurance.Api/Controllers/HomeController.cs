using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using Serilog;
using Insurance.Api.Models;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("api/insurance/product")]
        public ApiResponseModel CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            try
            {

                Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} started.");
                BusinessRules.GetProductType(ProductApi, ref toInsure);
                BusinessRules.GetSalesPrice(ProductApi, ref toInsure);

                if (toInsure.ProductTypeHasInsurance)
                {
                    if (toInsure.SalesPrice >= 500 && toInsure.SalesPrice < 2000)
                        toInsure.InsuranceValue += 1000;
                    if (toInsure.SalesPrice >= 2000)
                        toInsure.InsuranceValue += 2000;
                    if (toInsure.ProductTypeName == "Laptops" || toInsure.ProductTypeName == "Smartphones" || toInsure.ProductTypeName == "Digital cameras")
                        toInsure.InsuranceValue += 500;
                }
                Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} completed.");
                return new ApiResponseModel(ApiResponseState.Success, toInsure.InsuranceValue);

            }
            catch (Exception ex)
            {
                Log.Error($"Insurance value of {{ProductId = {toInsure.ProductId}}} could not be calculated\n{ex.Message}");
                return new ApiResponseModel(ApiResponseState.Error,ex.Message, ApiErrorCodeEnum.ProductNotFound);

            }
        }

        [HttpPost]
        [Route("api/insurance/productOrder")]
        public ApiResponseModel CalculateInsurance([FromBody] List<InsuranceDto> toInsure)
        {
            float insurance = 0f;
            int countOfDigitalCameras = 0;

            try
            {
                Log.Information($"Calculation of insurance for an order started. Count of products in the order: {toInsure.Count}");
                for (int i = 0; i < toInsure.Count; i++)
                {
                    CalculateInsurance(toInsure[i]);
                    if (toInsure[i].ProductTypeName == "Digital cameras") countOfDigitalCameras++;
                    insurance += toInsure[i].InsuranceValue;
                }
                if (countOfDigitalCameras > 0) insurance -= (countOfDigitalCameras - 1) * 500;

                Log.Information($"Calculation of insurance for an order completed.");
                return new ApiResponseModel(ApiResponseState.Success, insurance);

            }
            catch (Exception ex)
            {
                Log.Error($"Insurance of one or more items could not be calculated \n{ex.Message}");
                return new ApiResponseModel(ApiResponseState.Error, ex.Message, ApiErrorCodeEnum.ProductNotFound);
            }


        }

        public class InsuranceDto
        {
            public int ProductId { get; set; }
            public float InsuranceValue { get; set; }
            [JsonIgnore]
            public string ProductTypeName { get; set; }
            [JsonIgnore]
            public bool ProductTypeHasInsurance { get; set; }
            [JsonIgnore]
            public float SalesPrice { get; set; }
        }

        private const string ProductApi = "http://localhost:5002";
    }
}
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

        [HttpPost]
        [Route("api/insurance/product")]
        public ApiResponseModel CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            try
            {
                var ProductApi = _config.GetValue<string>("ProductApi");
                Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} started.");
                BusinessRules.GetProductType(ProductApi, ref toInsure);

                if (!toInsure.ProductTypeHasInsurance)
                    return new ApiResponseModel(ApiResponseState.Success, toInsure.InsuranceValue);

                BusinessRules.GetSalesPrice(ProductApi, ref toInsure);

                List<string> typeList = new List<string> {
                    StaticDataProvider.Laptops,
                    StaticDataProvider.DigitalCameras,
                    StaticDataProvider.SmartPhones};

                BusinessRules.CalculateInsuranceValue(ref toInsure, typeList);
                Log.Information($"Calculation of insurance for {{ProductId = {toInsure.ProductId}}} completed.");
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
                var ProductApi = _config.GetValue<string>("ProductApi");
                Log.Information($"Calculation of insurance for an order started. Id of products in the order: {string.Join("\n", toInsure.Select(x => x.ProductId))}");

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

                Log.Information($"Calculation of insurance for an order completed.");
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
        public async Task<ApiResponseModel> AddSurcharge([FromBody] InsuranceDto toInsure)
        {
            try
            {
                var ProductApi = _config.GetValue<string>("ProductApi"); 
                BusinessRules.GetProductType(ProductApi, ref toInsure);
                var surchargeRates = BusinessRules.ReadSurchargeFile();
                if (surchargeRates.Any(x => x.ProductId == toInsure.ProductId))
                    surchargeRates.Remove(surchargeRates.Find(x => x.ProductId == toInsure.ProductId));
                surchargeRates.Add(toInsure);
                BusinessRules.WriteSurchargeFile(surchargeRates);
                return new ApiResponseModel(ApiResponseState.Success, 0);
            }
            catch (Exception ex)
            {
                Log.Error($"There was an error while adding the Surcharge rate\n{ex.Message}");
                return new ApiResponseModel(ApiResponseState.Error, "There was an error while adding the Surcharge rate", ApiErrorCodeEnum.ProductNotFound);
            }
        }
    }
}
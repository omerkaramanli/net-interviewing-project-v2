using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Insurance.Api.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Microsoft.Extensions.Logging;
using Serilog;
using Insurance.Api.Dtos;

namespace Insurance.Api
{
    public static class BusinessRules
    {
        public static void GetProductType(string baseAddress, ref InsuranceDto insurance) 
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            try
            {
//                Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} started.");
                string json = client
                    .GetAsync(string.Format("/products/{0:G}", insurance.ProductId))
                    .Result.Content.ReadAsStringAsync()
                    .Result;

                var product = JsonConvert.DeserializeObject<dynamic>(json);

                json = client
                    .GetAsync(string.Format("/product_types/{0:G}", product.productTypeId))
                    .Result.Content.ReadAsStringAsync()
                    .Result;

                var productType = JsonConvert.DeserializeObject<dynamic>(json);

                insurance.ProductTypeName = productType.name;
                insurance.ProductTypeHasInsurance = productType.canBeInsured;
                //Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            //    throw new Exception($"Product type for {{ProductId = {insurance.ProductId}}} could not be found or product does not exist.");
            }


        }

        public static void GetSalesPrice(string baseAddress, ref InsuranceDto insurance) 
        {
            try
            {
            //    Log.Information($"Getting product sales price {{ProductId = {insurance.ProductId}}} started.");
                HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
                string json = client
                    .GetAsync(string.Format("/products/{0:G}", insurance.ProductId))
                    .Result.Content.ReadAsStringAsync()
                    .Result;

                var product = JsonConvert.DeserializeObject<dynamic>(json);

                insurance.SalesPrice = product.salesPrice;
                //Log.Information($"Getting product sales price {{ProductId = {insurance.ProductId}}} completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            //    throw new Exception($"Sales price for {{ProductId = {insurance.ProductId}}} could not be found.");
            }

        }

        public static void CalculateInsuranceValue(ref InsuranceDto insuranceDto, IList<string> typeList)
        {
            insuranceDto.InsuranceValue += CheckSalesPrice(insuranceDto.SalesPrice);
            insuranceDto.InsuranceValue += CheckProductType(insuranceDto.ProductTypeName, typeList);
        }

        private static float CheckSalesPrice(float salesPrice)
        {
            if (salesPrice >= 500 &&
                salesPrice < 2000)
            {
                return 1000;
            }
            else if (salesPrice >= 2000)
            {
                return 2000;
            }

            return 0;
        }

        private static float CheckProductType(string type, IList<string> typeList)
        {
            if (typeList.Any(x => x.Equals(type)))
            {
                return 500;
            }

            return 0;
        }
    }
}
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

namespace Insurance.Api
{
    public static class BusinessRules
    {
        public static void GetProductType(string baseAddress, ref HomeController.InsuranceDto insurance) //productId
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            //string json = client.GetAsync("/product_types").Result.Content.ReadAsStringAsync().Result;
            //var collection = JsonConvert.DeserializeObject<dynamic>(json);
            //instead of searching whole collection, we can just use product_types/{id}
            try
            {
                Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} started.");
                string json = client.GetAsync(string.Format("/products/{0:G}", insurance.ProductId)).Result.Content.ReadAsStringAsync().Result;
                var product = JsonConvert.DeserializeObject<dynamic>(json);

                json = client.GetAsync(string.Format("/product_types/{0:G}", product.productTypeId)).Result.Content.ReadAsStringAsync().Result;
                var productType = JsonConvert.DeserializeObject<dynamic>(json);

                insurance.ProductTypeName = productType.name;
                insurance.ProductTypeHasInsurance = productType.canBeInsured;
                Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"Product type for {{ProductId = {insurance.ProductId}}} could not be found or product does not exist.");
            }


            //int productTypeId = product.productTypeId;
            //string productTypeName = null;
            //bool hasInsurance = false;


            //insurance = new HomeController.InsuranceDto();

            /*for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].id == product.productTypeId && collection[i].canBeInsured == true)
                {
                    //insurance.ProductId = 
                    insurance.ProductTypeName = collection[i].name;
                    insurance.ProductTypeHasInsurance = true;
                }
            }*/
        }

        public static void GetSalesPrice(string baseAddress, ref HomeController.InsuranceDto insurance) //productId
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day, shared: true)
                .CreateLogger();
            try
            {
                Log.Information($"Getting product sales price {{ProductId = {insurance.ProductId}}} started.");
                HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
                string json = client.GetAsync(string.Format("/products/{0:G}", insurance.ProductId)).Result.Content.ReadAsStringAsync().Result;
                var product = JsonConvert.DeserializeObject<dynamic>(json);

                insurance.SalesPrice = product.salesPrice;
                Log.Information($"Getting product sales price {{ProductId = {insurance.ProductId}}} completed.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"Sales price for {{ProductId = {insurance.ProductId}}} could not be found.");
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Insurance.Api.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Insurance.Api.Dtos;
using System.IO;
using CsvHelper;
using System.Globalization;
using Insurance.Api.Interfaces;
using Insurance.Api.Models;

namespace Insurance.Api
{
    public class BusinessRules : IInsuranceService
    {
        public async Task<ApiResponseModel<InsuranceDto>> GetProductType(string baseAddress, InsuranceDto insurance)
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
            try
            {
                Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} started.");
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
                Log.Information($"Getting product type {{ProductId = {insurance.ProductId}}} completed.");
                return new ApiResponseModel<InsuranceDto>(ApiState.Success, insurance);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new ApiResponseModel<InsuranceDto>(ApiState.NotFound,$"Product type for {{ProductId = {insurance.ProductId}}} could not be found or product does not exist.");
            }


        }

        public async Task<ApiResponseModel<InsuranceDto>> GetSalesPrice(string baseAddress, InsuranceDto insurance)
        {
            try
            {
                Log.Information($"Getting product sales price {{ProductId = {insurance.ProductId}}} started.");
                HttpClient client = new HttpClient { BaseAddress = new Uri(baseAddress) };
                string json = client
                    .GetAsync(string.Format("/products/{0:G}", insurance.ProductId))
                    .Result.Content.ReadAsStringAsync()
                    .Result;

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

        public async Task<ApiResponseModel<InsuranceDto>> CalculateInsuranceValue(InsuranceDto insurance, IList<string> typeList)
        {
            insurance.InsuranceValue += CheckSalesPrice(insurance.SalesPrice);
            insurance.InsuranceValue += CheckProductType(insurance.ProductTypeName, typeList);
            insurance.InsuranceValue *= CheckSurchargeRate(insurance.ProductTypeName);
        }
        public async Task<ApiResponseModel<InsuranceDto>> AddSurcharge(InsuranceDto insurance)
        {

            var surchargeRates = BusinessRules.ReadSurchargeFile();
            surchargeRates = BusinessRules.UpdateLine(surchargeRates, insurance);
            BusinessRules.WriteSurchargeFile(surchargeRates);
        }


        private static float CheckSurchargeRate(string ProductTypeName)
        {
            try
            {
                var surchargeRates = ReadSurchargeFile();
                float surchargeRate = 0f;
                if (surchargeRates.Any(x => x.ProductTypeName.Equals(ProductTypeName)))
                    surchargeRate = surchargeRates.Find(x => x.ProductTypeName.Equals(ProductTypeName)).SurchargeRate;
                return 1 + (surchargeRate / 100);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"Surcharge rate for {{ProductTypeName = {ProductTypeName}}} could not be calculated.");
            }
        }

        private static List<InsuranceDto> ReadSurchargeFile()
        {
            try
            {
                var surchargeRates = new List<InsuranceDto>();
                if (!System.IO.File.Exists("SurchargeRates.csv"))
                {
                    WriteSurchargeFile(surchargeRates);
                }
                using (var reader = new StreamReader("SurchargeRates.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var surchargeRate = new InsuranceDto
                        {
                            ProductId = Int32.Parse(csv.GetField("ProductId")),
                            SurchargeRate = float.Parse(csv.GetField("SurchargeRate")),
                            ProductTypeName = csv.GetField("ProductTypeName")
                        };
                        surchargeRates.Add(surchargeRate);
                    }
                }
                return surchargeRates;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"There was an error while reading Surcharge file.");
            }
        }
        private static void WriteSurchargeFile(List<InsuranceDto> surchargeRates)
        {
            try
            {
                using (var writer = new StreamWriter("SurchargeRates.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(surchargeRates);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"There was an error while writing Surcharge file.");
            }
        }
        private static List<InsuranceDto> UpdateLine(List<InsuranceDto> surchargeRates, InsuranceDto toInsure)
        {
            if(surchargeRates.Any(x => x.ProductTypeName.Equals(toInsure.ProductTypeName)))
                    surchargeRates.Remove(surchargeRates.Find(x => x.ProductTypeName.Equals(toInsure.ProductTypeName)));
            surchargeRates.Add(toInsure);
            return surchargeRates;
        }

        private static float CheckSalesPrice(float salesPrice)
        {
            if (salesPrice >= StaticDataProvider.SalesPriceFirstThreshold &&
                salesPrice < StaticDataProvider.SalesPriceSecondThreshold)
            {
                return StaticDataProvider.FirstInsuranceValue;
            }
            else if (salesPrice >= StaticDataProvider.SalesPriceSecondThreshold)
            {
                return StaticDataProvider.SecondInsuranceValue;
            }

            return 0;
        }

        private static float CheckProductType(string type, IList<string> typeList)
        {
            if (typeList.Any(x => x.Equals(type)))
            {
                if (type == StaticDataProvider.Laptops)
                    return StaticDataProvider.LaptopsAdditionalInsuranceValue;

                if (type == StaticDataProvider.SmartPhones)
                    return StaticDataProvider.SmartPhonesAdditionalInsuranceValue;

                if (type == StaticDataProvider.DigitalCameras)
                    return StaticDataProvider.DigitalCamerasAdditionalInsuranceValue;
            }

            return 0;
        }
    }
}
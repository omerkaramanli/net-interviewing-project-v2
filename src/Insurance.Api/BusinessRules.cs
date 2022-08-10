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

namespace Insurance.Api
{
    public static class BusinessRules
    {
        public static void GetProductType(string baseAddress, ref InsuranceDto insurance)
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
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"Product type for {{ProductId = {insurance.ProductId}}} could not be found or product does not exist.");
            }


        }

        public static void GetSalesPrice(string baseAddress, ref InsuranceDto insurance)
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

        public static void CalculateInsuranceValue(ref InsuranceDto insuranceDto, IList<string> typeList)
        {
            insuranceDto.InsuranceValue += CheckSalesPrice(insuranceDto.SalesPrice);
            insuranceDto.InsuranceValue += CheckProductType(insuranceDto.ProductTypeName, typeList);
            insuranceDto.InsuranceValue *= CheckSurchargeRate(insuranceDto.ProductId);
        }

        private static float CheckSurchargeRate(int productId)
        {
            try
            {
                var surchargeRates = ReadSurchargeFile();
                float surchargeRate = 0f;
                if(surchargeRates.Any(x=>x.ProductId == productId))
                    surchargeRate = surchargeRates.Find(x => x.ProductId == productId).SurchargeRate;
                return 1 + (surchargeRate / 100);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"Surcharge rate for {{ProductId = {productId}}} could not be calculated.");
            }
        }

        public static List<InsuranceDto> ReadSurchargeFile()
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
                            SurchargeRate = float.Parse(csv.GetField("SurchargeRate"))
                        };
                        surchargeRates.Add(surchargeRate);
                    }
                }
                return surchargeRates;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"There was an error while reading Surcharge file.");
            }
        }
        public static void WriteSurchargeFile(List<InsuranceDto> surchargeRates)
        {
            try
            {
                using (var writer = new StreamWriter("SurchargeRates.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(surchargeRates);
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception($"There was an error while writing Surcharge file.");
            }
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
                if(type == StaticDataProvider.Laptops)
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
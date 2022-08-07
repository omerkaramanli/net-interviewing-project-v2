using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("api/insurance/product")]
        public InsuranceDto CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            //int productId = toInsure.ProductId;
            try
            {
                BusinessRules.GetProductType(ProductApi, ref toInsure);
                BusinessRules.GetSalesPrice(ProductApi, ref toInsure);

                //float insurance = 0f;

                //simplified the if else block
                //toInsure.InsuranceValue = 0;
                if (toInsure.ProductTypeHasInsurance)
                {
                    if (toInsure.SalesPrice >= 500 && toInsure.SalesPrice < 2000)
                        toInsure.InsuranceValue += 1000;
                    if (toInsure.SalesPrice >= 2000)
                        toInsure.InsuranceValue += 2000;
                    if (toInsure.ProductTypeName == "Laptops" || toInsure.ProductTypeName == "Smartphones")
                        toInsure.InsuranceValue += 500;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (toInsure != null)
                return toInsure;
            else
                throw new Exception("Insurance value could not be calculated");
        }

        public float CalculateInsurance([FromBody] List<InsuranceDto> toInsure)
        {
            float insurance = 0f;
            bool atLeastOneDigitalCamera = false;

            try
            {
                for (int i = 0; i < toInsure.Count; i++)
                {
                    CalculateInsurance(toInsure[i]);
                    if (toInsure[i].ProductTypeName == "Digital cameras") atLeastOneDigitalCamera = true;
                    insurance += toInsure[i].InsuranceValue;
                }
                if (atLeastOneDigitalCamera) insurance += 500;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Insurance of one or more items could not be calculated");
            }

            return insurance;
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
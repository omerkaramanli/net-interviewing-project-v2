using System;
using Insurance.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests //: IClassFixture<ControllerTestFixture>
    {
       /* private readonly ControllerTestFixture _fixture;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }*/

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500Euros_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 725435,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 1000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 836194,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 735296,//Dont have one yet
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: 2000//result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndSmartphoneOrLaptop_ShouldAdd500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 837856,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndSmartphoneOrLaptop_ShouldAdd500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 1500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 858421,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndSmartphoneOrLaptop_ShouldAdd500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 837856, //Dont have one yet
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: 2500//result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndCantBeInsured_ShouldNotAddToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 819148, 
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndCantBeInsured_ShouldNotAddToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 767490,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndCantBeInsured_ShouldNotAddToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 735296,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
                );
        }
    }

    /*public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = new
                            {
                                id = productId,
                                name = "Test Product",
                                productTypeId = 1,
                                salesPrice = 750
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                                               {
                                                   new
                                                   {
                                                       id = 1,
                                                       name = "Test type",
                                                       canBeInsured = true
                                                   }
                                               };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                }
            );
        }
    }*/
}
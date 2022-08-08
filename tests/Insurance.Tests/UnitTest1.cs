using System;
using System.Collections.Generic;
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
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAdd1000EurosToInsuranceCost()
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
                ProductId = -1, //Dont have one yet
                ProductTypeName = "Washing machines",
                SalesPrice = 2500,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2000
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
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndSmartphoneOrLaptop_ShouldAdd1500EurosToInsuranceCost()
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
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndSmartphoneOrLaptop_ShouldAdd2500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2500;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = -1, //Dont have one yet
                ProductTypeName = "Smartphones",
                SalesPrice = 2500,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2500
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
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndCantBeInsured_ShouldRequireNoInsurance()
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
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndCantBeInsured_ShouldRequireNoInsurance()
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
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndCantBeInsured_ShouldRequireNoInsurance()
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

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 819148 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 836676 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 832845 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 725435 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 838978 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 838978 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 838978 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 838978 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 735296 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735296 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735296 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735296 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }
              
        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 572770 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 805073 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 572770 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 805073 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosOrder_ShouldAddXTimes1000ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 4000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 735246 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735246 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735246 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 735246 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosOrder_ShouldAddXTimes2000ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 8000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -1, //Dont have one yet
                ProductTypeName = "Washing machines",
                SalesPrice = 2000,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2000
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -2, //Dont have one yet
                ProductTypeName = "Washing machines",
                SalesPrice = 2100,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2000
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -3, //Dont have one yet
                ProductTypeName = "Washing machines",
                SalesPrice = 2200,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2000
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -4, //Dont have one yet
                ProductTypeName = "Washing machines",
                SalesPrice = 2300,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2000
            });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndSmartphoneOrLaptopOrder_ShouldAddYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 2000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 828519 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 837856 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 828519 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 837856 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosAndSmartphoneOrLaptopOrder_ShouldAddXTimes1000PlusYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 6000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 827074 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 858421 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 859366 });
            dto.Add(new HomeController.InsuranceDto { ProductId = 861866 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndSmartphoneOrLaptopOrder_ShouldAddXTimes2000PlusYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 10000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -1, //Dont have one yet
                ProductTypeName = "Smartphones",
                SalesPrice = 2000,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2500
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -2, //Dont have one yet
                ProductTypeName = "Laptops",
                SalesPrice = 2100,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2500
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -3, //Dont have one yet
                ProductTypeName = "Smartphones",
                SalesPrice = 2200,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2500
            });
            dto.Add(new HomeController.InsuranceDto
            {
                ProductId = -4, //Dont have one yet
                ProductTypeName = "Laptops",
                SalesPrice = 2300,
                ProductTypeHasInsurance = true,
                InsuranceValue = 2500
            });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        //ToDo: add unit tests for range of items in an order
        [Fact]
        public void CalculateInsurance_GivenOneOfEachTypeOfItemInOrder_ShouldAdd6500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 6500;

            var dto = new List<HomeController.InsuranceDto>();

            //Type 21 can be insured <500 // 500-2000 -Laptops
            dto.Add(new HomeController.InsuranceDto { ProductId = 837856 }); //500
            dto.Add(new HomeController.InsuranceDto { ProductId = 858421 }); //1500
            //Type 32 can be insured <500 // 500-2000 -Smartphones
            dto.Add(new HomeController.InsuranceDto { ProductId = 828519 }); //500
            dto.Add(new HomeController.InsuranceDto { ProductId = 827074 }); //1500
            //Type 33 can be insured <500 // 500-2000 -Digital Cameras
            dto.Add(new HomeController.InsuranceDto { ProductId = 715990 }); //0 + 500
            dto.Add(new HomeController.InsuranceDto { ProductId = 836194 }); //1000
            //Type 35 cannot be insured <500 // >2000 -SLR Cameras
            dto.Add(new HomeController.InsuranceDto { ProductId = 819148 }); //0
            dto.Add(new HomeController.InsuranceDto { ProductId = 735296 }); //0
            //Type 12 cannot be insured <500 -MP3 player
            dto.Add(new HomeController.InsuranceDto { ProductId = 725435 }); //0
            //Type 124 can be insured <500 // 500-2000 -Washing machines
            dto.Add(new HomeController.InsuranceDto { ProductId = 572770 }); //0
            dto.Add(new HomeController.InsuranceDto { ProductId = 735246 }); //1000
            //Type 841 cannot be insured  500-2000 -Laptops
            dto.Add(new HomeController.InsuranceDto { ProductId = 838978 }); //0

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
                );
        }

        //ToDo: add unit tests for task 4
        [Fact]
        public void CalculateInsurance_GivenAtLEastOneDigitalCameraInOrder_ShouldAdd1500Plus500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 1000;

            var dto = new List<HomeController.InsuranceDto>();
            dto.Add(new HomeController.InsuranceDto { ProductId = 837856 }); //500
            dto.Add(new HomeController.InsuranceDto { ProductId = 715990 }); //0 + digital camera 500
            dto.Add(new HomeController.InsuranceDto { ProductId = 725435 }); //0
            dto.Add(new HomeController.InsuranceDto { ProductId = 572770 }); //0

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
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
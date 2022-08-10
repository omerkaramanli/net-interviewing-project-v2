using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Controllers;
using Insurance.Api.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500Euros_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new InsuranceDto
            {
                ProductId = 1,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAdd1000EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 1000;

            var dto = new InsuranceDto
            {
                ProductId = 12,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2000;

            var dto = new InsuranceDto
            {
                ProductId = 13, 
                
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndSmartphoneOrLaptop_ShouldAdd500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 500;

            var dto = new InsuranceDto
            {
                ProductId = 3,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndSmartphoneOrLaptop_ShouldAdd1500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 1500;

            var dto = new InsuranceDto
            {
                ProductId = 32,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndSmartphoneOrLaptop_ShouldAdd2500EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2500;

            var dto = new InsuranceDto
            {
                ProductId = 33,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndCantBeInsured_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new InsuranceDto
            {
                ProductId = 2, 
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000EurosAndCantBeInsured_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new InsuranceDto
            {
                ProductId = 22,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndCantBeInsured_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new InsuranceDto
            {
                ProductId = 23,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 2 });
            dto.Add(new InsuranceDto { ProductId = 4 });
            dto.Add(new InsuranceDto { ProductId = 6 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 22 });
            dto.Add(new InsuranceDto { ProductId = 42 });
            dto.Add(new InsuranceDto { ProductId = 62 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndCantBeInsuredOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 23 });
            dto.Add(new InsuranceDto { ProductId = 43 });
            dto.Add(new InsuranceDto { ProductId = 63 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }
              
        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosOrder_ShouldRequireNoInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 1 });
            dto.Add(new InsuranceDto { ProductId = 1 });
            dto.Add(new InsuranceDto { ProductId = 1 });
            dto.Add(new InsuranceDto { ProductId = 1 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosOrder_ShouldAddXTimes1000ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 4000;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 12 });
            dto.Add(new InsuranceDto { ProductId = 12 });
            dto.Add(new InsuranceDto { ProductId = 12 });
            dto.Add(new InsuranceDto { ProductId = 12 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosOrder_ShouldAddXTimes2000ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 8000;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto
            {
                ProductId = 13
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 13
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 13
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 13
            });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBelow500EurosAndSmartphoneOrLaptopOrder_ShouldAddYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 2000;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 3 });
            dto.Add(new InsuranceDto { ProductId = 5 });
            dto.Add(new InsuranceDto { ProductId = 3 });
            dto.Add(new InsuranceDto { ProductId = 5 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500and2000EurosAndSmartphoneOrLaptopOrder_ShouldAddXTimes1000PlusYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 6000;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 32 });
            dto.Add(new InsuranceDto { ProductId = 52 });
            dto.Add(new InsuranceDto { ProductId = 32 });
            dto.Add(new InsuranceDto { ProductId = 52 });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceAbove2000EurosAndSmartphoneOrLaptopOrder_ShouldAddXTimes2000PlusYTimes500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 10000;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto
            {
                ProductId = 33
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 53
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 33
            });
            dto.Add(new InsuranceDto
            {
                ProductId = 53
            });

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        //ToDo: add unit tests for range of items in an order
        [Fact]
        public void CalculateInsurance_GivenOneOfEachTypeOfItemInOrder_ShouldAdd6500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 15500;

            var dto = new List<InsuranceDto>();

            dto.Add(new InsuranceDto { ProductId = 1  }); //0
            dto.Add(new InsuranceDto { ProductId = 12 }); //1000
            dto.Add(new InsuranceDto { ProductId = 13 }); //2000
            dto.Add(new InsuranceDto { ProductId = 2  }); //0
            dto.Add(new InsuranceDto { ProductId = 22 }); //0
            dto.Add(new InsuranceDto { ProductId = 23 }); //0
            dto.Add(new InsuranceDto { ProductId = 3  }); //500
            dto.Add(new InsuranceDto { ProductId = 32 }); //1500
            dto.Add(new InsuranceDto { ProductId = 33 }); //2500
            dto.Add(new InsuranceDto { ProductId = 4  }); //0
            dto.Add(new InsuranceDto { ProductId = 42 }); //0
            dto.Add(new InsuranceDto { ProductId = 43 }); //0
            dto.Add(new InsuranceDto { ProductId = 5  }); //500
            dto.Add(new InsuranceDto { ProductId = 52 }); //1500
            dto.Add(new InsuranceDto { ProductId = 53 }); //2500
            dto.Add(new InsuranceDto { ProductId = 6  }); //0
            dto.Add(new InsuranceDto { ProductId = 62 }); //0
            dto.Add(new InsuranceDto { ProductId = 63 }); //0
            dto.Add(new InsuranceDto { ProductId = 7  }); //0    + 500
            dto.Add(new InsuranceDto { ProductId = 72 }); //1000 
            dto.Add(new InsuranceDto { ProductId = 73 }); //2000 
            dto.Add(new InsuranceDto { ProductId = 8  }); //0
            dto.Add(new InsuranceDto { ProductId = 82 }); //0
            dto.Add(new InsuranceDto { ProductId = 83 }); //0

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        //ToDo: add unit tests for task 4
        [Fact]
        public void CalculateInsurance_GivenOneDigitalCameraBelow500Euros_ShouldAdd500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 500;

            var dto = new InsuranceDto
            {
                ProductId = 7,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }
        [Fact]
        public void CalculateInsurance_GivenOneDigitalCameraBetween500and2000Euros_ShouldAdd1500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 1500;

            var dto = new InsuranceDto
            {
                ProductId = 72,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }
        [Fact]
        public void CalculateInsurance_GivenOneDigitalCameraAbove2000Euros_ShouldAdd2500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 2500;

            var dto = new InsuranceDto
            {
                ProductId = 73,
            };

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }
        [Fact]
        public void CalculateInsurance_GivenAtLEastOneDigitalCameraInOrder_ShouldAdd1500Plus500ToInsurance()
        {
            //Arrange
            const float expectedInsuranceValue = 2500;

            var dto = new List<InsuranceDto>();
            dto.Add(new InsuranceDto { ProductId = 12 }); //500
            dto.Add(new InsuranceDto { ProductId = 7 }); //0 + digital camera 500
            dto.Add(new InsuranceDto { ProductId = 72 }); //1000 digital camera + 500
            dto.Add(new InsuranceDto { ProductId = 1 }); //0

            //Act
            var sut = new HomeController();
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }


        [Fact]
        public async void CalculateInsurance_GivenSurcharge10PercentForGivenSalesPriceBelow500Euros_ShouldRequireNoInsuranceAsync()
        {
            //Arrange
            const float expectedInsuranceValue = 0;

            var dto = new InsuranceDto
            {
                ProductId = 9,
                SurchargeRate = 10
            };

            //Act
            var sut = new HomeController();
            await sut.AddSurcharge(dto);
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public async void CalculateInsurance_GivenSurcharge10PercentForGivenSalesPriceBetween500And2000Euros_ShouldAdd1000EurosToInsuranceCostAsync()
        {
            //Arrange
            const float expectedInsuranceValue = 1100;

            var dto = new InsuranceDto
            {
                ProductId = 92,
                SurchargeRate = 10
            };

            //Act
            var sut = new HomeController();
            await sut.AddSurcharge(dto);
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }

        [Fact]
        public async void CalculateInsurance_GivenSurcharge10PercentForGivenSalesPriceAbove2000Euros_ShouldAdd2000EurosToInsuranceCost()
        {
            //Arrange
            const float expectedInsuranceValue = 2200;

            var dto = new InsuranceDto
            {
                ProductId = 93,
                SurchargeRate = 10
            };

            //Act
            var sut = new HomeController();
            await sut.AddSurcharge(dto);
            var result = sut.CalculateInsurance(dto);

            //Assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.Result
                );
        }
    }

    public class ControllerTestFixture : IDisposable
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
                            var products = new[]
                                              {
                                new
                                {
                                    id = 9,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 92,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 1250
                                },
                                new
                                {
                                    id = 93,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 2250
                                },
                                new
                                {
                                    id = 1,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 12,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 1250
                                },
                                new
                                {
                                    id = 13,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 2250
                                },
                                new
                                {
                                    id = 2,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 22,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 1250
                                },
                                new
                                {
                                    id = 23,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 3,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 32,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 33,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 4,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 42,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 43,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 5,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 52,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 53,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 6,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 62,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 63,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 7,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 72,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 73,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 8,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 82,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 83,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 2250
                                },
                            };

                            for (int i = 0; i < products.Length; i++)
                            {
                                if (products[i].id == productId)
                                {
                                    return context.Response.WriteAsync(JsonConvert.SerializeObject(products[i]));

                                }
                            }

                            return context.Response.WriteAsync(JsonConvert.SerializeObject("Not found"));
                        }
                    );
                    ep.MapGet(
                        "products",
                        context =>
                        {
                            var products = new[]
                                               {
                                new
                                {
                                    id = 1,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 12,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 1250
                                },
                                new
                                {
                                    id = 13,
                                        name = "Test Product",
                                        productTypeId = 1,
                                        salesPrice = 2250
                                },
                                new
                                {
                                    id = 2,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 22,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 1250
                                },
                                new
                                {
                                    id = 23,
                                        name = "Test Product",
                                        productTypeId = 2,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 3,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 32,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 33,
                                        name = "Laptops",
                                        productTypeId = 3,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 4,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 42,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 43,
                                        name = "Laptops",
                                        productTypeId = 4,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 5,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 52,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 53,
                                        name = "Smartphones",
                                        productTypeId = 5,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 6,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 62,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 63,
                                        name = "Smartphones",
                                        productTypeId = 6,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 7,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 72,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 73,
                                        name = "Digital cameras",
                                        productTypeId = 7,
                                        salesPrice = 2250
                                },
                                new
                                {
                                   id = 8,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 250
                                },
                                new
                                {
                                   id = 82,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 1250
                                },
                                new
                                {
                                   id = 83,
                                        name = "Digital cameras",
                                        productTypeId = 8,
                                        salesPrice = 2250
                                },
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(products));
                        }
                    );
                    ep.MapGet(
                        "product_types/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var productTypes = new[]
                                               {
                            new
                            {
                                id = 1,
                                name = "Test type",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 2,
                                name = "Test type",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 3,
                                name = "Laptops",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 4,
                                name = "Laptops",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 5,
                                name = "Smartphones",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 6,
                                name = "Smarthphones",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 7,
                                name = "Digital cameras",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 8,
                                name = "Digital cameras",
                                canBeInsured = false
                            },
                        };
                            for (int i = 0; i < productTypes.Length; i++)
                            {
                                if (productTypes[i].id == productId)
                                {
                                    return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes[i]));

                                }
                            }

                            return context.Response.WriteAsync(JsonConvert.SerializeObject("Not found"));
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
                            },
                            new
                            {
                                id = 2,
                                name = "Test type",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 3,
                                name = "Laptops",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 4,
                                name = "Laptops",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 5,
                                name = "Smartphones",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 6,
                                name = "Smarthphones",
                                canBeInsured = false
                            },
                            new
                            {
                                id = 7,
                                name = "Digital cameras",
                                canBeInsured = true
                            },
                            new
                            {
                                id = 8,
                                name = "Digital cameras",
                                canBeInsured = false
                            },
                        };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );

                }
            );
        }
    }
}
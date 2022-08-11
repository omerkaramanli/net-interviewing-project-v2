using Insurance.Api.Models;
using Insurance.Api.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Insurance.Api.Interfaces
{
    public interface IInsuranceService
    {
        Task<ApiResponseModel<InsuranceDto>> GetProductType(string baseAddress, ref InsuranceDto insurance);
        Task<ApiResponseModel<InsuranceDto>> GetSalesPrice(string baseAddress, ref InsuranceDto insurance);
        Task<ApiResponseModel<InsuranceDto>> CalculateInsuranceValue(ref InsuranceDto insurance, IList<string> typeList);
        Task<ApiResponseModel<InsuranceDto>> AddSurcharge(InsuranceDto insurance);
    }
}

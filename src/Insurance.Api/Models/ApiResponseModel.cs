namespace Insurance.Api.Models
{
    public class ApiResponseModel
    {

        public ApiResponseModel(ApiResponseState stateCode, string errorMessage, ApiErrorCodeEnum errorCode)
        {
            StateCode = stateCode; 
            ErrorMessage = errorMessage; 
            ErrorCode = errorCode;
        }

        public ApiResponseModel(ApiResponseState stateCode, float result)
        {
            StateCode = stateCode;
            Result = result;
        }
        public ApiResponseState StateCode { get; set; }  
        public string ErrorMessage { get; set; }    
        public ApiErrorCodeEnum ErrorCode { get; set; }
        public float Result { get; set; }   

    }
}

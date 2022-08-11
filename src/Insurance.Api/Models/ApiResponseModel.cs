namespace Insurance.Api.Models
{
    public class ApiResponseModel<T>
    {
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public ApiState Status { get; set; }

        public ApiResponseModel(ApiState apiState, string errorMessage)
        {
            Status = apiState;
            ErrorMessage = errorMessage;
        }

        public ApiResponseModel(ApiState apiState, T result)
        {
            Status = apiState;
            Result = result;
        }

    }
}

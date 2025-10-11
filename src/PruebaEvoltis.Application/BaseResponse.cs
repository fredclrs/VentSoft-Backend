
namespace Application
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public BaseResponse() { }

        public BaseResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static BaseResponse<T> SuccessResponse(T data, string message = "Operación exitosa.")
            => new BaseResponse<T>(true, message, data);

        public static BaseResponse<T> FailureResponse(string message)
            => new BaseResponse<T>(false, message);
    }

}

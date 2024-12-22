namespace JWTAuthentication.Library.Models.Dto
{

    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public string? Message { get; set; }
    }
    public static class ResponseDtoExtension
    {
        public static ResponseDto GetResponseSuccess(string msg = "Success")
        {
            return new ResponseDto()
            {
                IsSuccess = true,
                Message = msg
            };
        }
        public static ResponseDto GetResponseFail(string? msg)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = msg
            };
        }
    }

}

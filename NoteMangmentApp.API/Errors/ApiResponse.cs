namespace NoteMangmentApp.API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You Made a Bad Request!",
                401 => "You are Not Authorized!",
                404 => "Resource Not Found!",
                500 => "Server Error Happened!",
                _ => null
            };
        }
    }
}

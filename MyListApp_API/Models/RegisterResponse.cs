namespace MyListApp_API.Models
{
    public class RegisterResponse
    {
        public string Message { get; set; }
    }

    public class ModelStateErrorResponse
    {
        public Dictionary<string, List<string>> Errors { get; set; }
    }

    public class ErrorResponse
    {
 
        public Dictionary<string, List<string>> Errors { get; set; }
    }


}

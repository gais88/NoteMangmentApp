namespace NoteMangmentApp.API.ViewModels
{
    public class MemberVM
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        
    }
}

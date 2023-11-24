namespace NoteMangmentApp.API.ViewModels
{
    public class UpdateNoteVM
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string? CurrentImage { get; set; }
    }
}

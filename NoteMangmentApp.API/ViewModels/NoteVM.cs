namespace NoteMangmentApp.API.ViewModels
{
    public class NoteVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string NoteUrl { get; set; } = string.Empty;
    }
}

namespace NoteManagmentApp.UI.ViewModels
{
    public class UpdateNoteVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public string? CurrentImage { get; set; }
    }
}

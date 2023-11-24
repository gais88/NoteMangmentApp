﻿namespace NoteManagmentApp.UI.ViewModels
{
    public class CreateNoteVM
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}

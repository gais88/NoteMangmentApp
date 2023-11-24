using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; }=string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string NoteUrl { get; set; } = string.Empty;
        

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = default!;
        public bool IsDeleted { get; set; }
        
    }
}

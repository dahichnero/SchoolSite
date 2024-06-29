using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class UpdateEventViewModel
    {
        public int Id { get; set; }
        public string NameEvent { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public int Smena { get; set; }

        public int NumLesson { get; set; }

        public DateOnly Date { get; set; }

        public List<SelectListItem> Teachers { get; set; } = new();
    }
}

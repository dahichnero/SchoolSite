using Microsoft.AspNetCore.Mvc.Rendering;
using SchkalkaB.Models;
using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class AddEventViewModel
    {

        public string NameEvent { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public int Smena { get; set; }

        public int NumLesson { get; set; }

        public DateOnly Date { get; set; }

        public List<SelectListItem> Teachers { get; set; } = new();
    }
}

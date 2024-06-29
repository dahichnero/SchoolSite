using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class AddTeacherViewModel
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string SurName { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int GenderId { get; set; }

        public DateOnly Birth { get; set; }


        public List<SelectListItem> Genders { get; set; } = new();

        public List<SelectListItem> Users { get; set; } = new();
    }
}

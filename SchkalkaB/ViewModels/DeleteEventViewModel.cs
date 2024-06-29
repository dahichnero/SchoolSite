using Microsoft.AspNetCore.Mvc.Rendering;
using SchkalkaB.Models;
using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class DeleteEventViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Название")]
        public string NameEvent { get; set; }=string.Empty;

        [Required]
        [Display(Name = "Учитель")]
        public int TeacherId { get; set; }

        [Display(Name = "Смена")]
        public int Smena { get; set; }

        [Display(Name = "Номер урока")]
        public int NumLesson { get; set; }

        [Display(Name = "Дата")]
        public string Date { get; set; }=string.Empty;

        public Teacher? Teacher { get; set; }
    }
}

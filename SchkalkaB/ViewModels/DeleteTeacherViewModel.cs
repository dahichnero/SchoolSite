using SchkalkaB.Models;
using System.ComponentModel.DataAnnotations;

namespace SchkalkaB.ViewModels
{
    public class DeleteTeacherViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string SurName { get; set; } = string.Empty;

        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        [Display(Name = "Пол")]
        public int GenderId { get; set; }

        [Display(Name = "Дата рождения")]
        public string Birth { get; set; } = string.Empty;

        public User? User { get; set; }
        public Gender? Gender { get; set; }
    }
}

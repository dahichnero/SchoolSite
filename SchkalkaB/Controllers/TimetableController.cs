using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchkalkaB.Data;
using SchkalkaB.Models;
using System.Text.Json;
using System;
using SchkalkaB.Domain.Services;
using SchkalkaB.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.Xml;

namespace SchkalkaB.Controllers
{
    public class TimetableController : Controller
    {
        private readonly ITimetable timetable;
        private readonly ITimetableService timetableService;
        SchkalkaDbContext context=new SchkalkaDbContext();
        int day;
        public  TimetableController(ITimetable timetable, ITimetableService timetableService)
        {
            
            this.timetable = timetable;
            this.timetableService = timetableService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            using (StreamReader reader = new StreamReader(@"add.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            var viewModel = new TimetableViewModel
            {
                timeTables = await timetable.TimeTableStudentAsync(day),
                Student = await timetable.GetStudentAsync(),
                DateMy=DateTime.Today.AddDays(day)
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> IndexTeacher()
        {
            using (StreamReader reader = new StreamReader(@"add1.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            var viewModel = new TimetableViewModel
            {
                FirstSmena = await timetable.TimeTableTeacherAsync(day, 1),
                LastSmena = await timetable.TimeTableTeacherAsync(day, 2),
                Teacher=await timetable.GetTeacherAsync(),
                DateMy = DateTime.Today.AddDays(day)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Plus()
        {
            
            using (StreamReader reader = new StreamReader(@"add.txt"))
            {
                day=int.Parse(reader.ReadLine());
            }
            day++;
            using (StreamWriter writer = new StreamWriter(@"add.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("Index", "Timetable"); 
        }

        public async Task<IActionResult> Minus()
        {
            
            using (StreamReader reader = new StreamReader(@"add.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            day--;
            using (StreamWriter writer = new StreamWriter(@"add.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("Index", "Timetable");
        }

        public async Task<IActionResult> PlusT()
        {

            using (StreamReader reader = new StreamReader(@"add1.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            day++;
            using (StreamWriter writer = new StreamWriter(@"add1.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("IndexTeacher", "Timetable");
        }

        public async Task<IActionResult> MinusT()
        {

            using (StreamReader reader = new StreamReader(@"add1.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            day--;
            using (StreamWriter writer = new StreamWriter(@"add1.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("IndexTeacher", "Timetable");
        }



        [Authorize]
        public async Task<IActionResult> IndexDirector()
        {
            using (StreamReader reader = new StreamReader(@"add2.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            var viewModel = new TimetableViewModel
            {
                AllTimetables = await timetable.TimeTableDirectorAsync(day),
                Director=await timetable.GetDirectorAsync(),
                Classes=await timetable.GetClassesAsync(),
                DateMy = DateTime.Today.AddDays(day)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> PlusD()
        {

            using (StreamReader reader = new StreamReader(@"add2.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            day++;
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("IndexDirector", "Timetable");
        }

        public async Task<IActionResult> MinusD()
        {

            using (StreamReader reader = new StreamReader(@"add2.txt"))
            {
                day = int.Parse(reader.ReadLine());
            }
            day--;
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(day);
            }
            return RedirectToAction("IndexDirector", "Timetable");
        }

        [Authorize]
        public async Task<IActionResult> MainAsync()
        {
            User user;
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                user = await JsonSerializer.DeserializeAsync<User>(fs);
            }
            List<Teacher> teachers = context.Teachers.ToList();
            List<Director> directors = context.Directors.ToList();
            Teacher? found = teachers.FirstOrDefault(u => u.Userl == user?.UserId);
            Director? found1 = directors.FirstOrDefault(u => u.UserI == user?.UserId);
            if (found != null)
            {
                return RedirectToAction("IndexTeacher", "Timetable");
            }
            else if (found1 != null)
            {
                return RedirectToAction("IndexDirector", "Timetable");
            }
            return RedirectToAction("Index", "Timetable");
        }

        [Authorize]
        public async Task<IActionResult> Parents()
        {
            using (StreamWriter writer = new StreamWriter(@"add.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new TimetableViewModel
            {
                Mother = await timetable.GetMotherAsync(),
                Father=await timetable.GetFatherAsync(),
                Class=await timetable.GetClassAsync(),
                Student = await timetable.GetStudentAsync(),
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Homework(int timetableId)
        {
            TimeTable timetablee = await timetable.FindTimeTableAsync(timetableId);
            if (timetablee is null)
            {
                return NotFound();
            }
            TeacherSubject teacherSubject=context.TeacherSubjects.FirstOrDefault(z=>z.TeacherSubjectId==timetablee.TeacherSubject);
            var viewModel = new AddHomeworkViewModel
            {
                TimeTableId=timetablee.TimeTableId,
                Homework=timetablee.HomeWork,
                DayOfWeek=timetablee.DayOfWeek,
                NumLesson=timetablee.NumLesson,
                Smena=timetablee.Smena,
                Cabinet=timetablee.Cabinet,
                TeacherSubject=timetablee.TeacherSubject,
                Class=timetablee.Class,

            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Homework(AddHomeworkViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            TimeTable timeTablee = await timetable.FindTimeTableAsync(viewModel.TimeTableId);
            if (timeTablee is null)
            {
                ModelState.AddModelError("not_found", "Расписание не найдено");
                return View(timeTablee);
            }
            try
            {
                timeTablee.TimeTableId = viewModel.TimeTableId;
                timeTablee.NumLesson = viewModel.NumLesson;
                timeTablee.HomeWork = viewModel.Homework;
                timeTablee.DayOfWeek=viewModel.DayOfWeek;
                timeTablee.Smena= viewModel.Smena;
                timeTablee.Cabinet= viewModel.Cabinet;
                timeTablee.TeacherSubject= viewModel.TeacherSubject;
                timeTablee.Class= viewModel.Class;
                //await timetableService.UpdateTimetable(timeTablee);
                context.TimeTables.Update(timeTablee);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных");
                return View(viewModel);
            }
            return RedirectToAction("IndexTeacher","Timetable");
        }

        [Authorize]
        public async Task<IActionResult> Events()
        {
            using (StreamWriter writer = new StreamWriter(@"add1.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new TimetableViewModel
            {
                Teacher= await timetable.GetTeacherAsync(),
                Events=await timetable.GetEventsAsync(),
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Adding()
        {
            using (StreamWriter writer = new StreamWriter(@"add1.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new TimetableViewModel
            {
                Teacher=await timetable.GetTeacherAsync(),
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> CheckStudents()
        {
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new TimetableViewModel
            {
                Director=await timetable.GetDirectorAsync(),
                Students=await timetable.GetStudentsAsync(),
            };
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddEvent()
        {
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new AddEventViewModel();
            var teachers = await timetable.GetTeachersAsync();

            var items = teachers.Select(c =>
            new SelectListItem { Text = c.TeacherName+" "+c.TeacherLastName+" "+c.TeacherSurName, Value = c.TeacherId.ToString() });

            viewModel.Teachers.AddRange(items);
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEvent(AddEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            DateOnly test = viewModel.Date;
            if (test < DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }//тута
            bool exist = context.Events.Any(z => z.NameEvent == viewModel.NameEvent && z.NumLesson == viewModel.NumLesson && z.Smena == viewModel.Smena && z.Teacher == viewModel.TeacherId
            && z.Date==viewModel.Date);
            if (exist)
            {
                ModelState.AddModelError("exist", "уже существует, поэтому нет необходимости создавать новый элемент, который портит базу данных");
                return View(viewModel);
            }
            try
            {
                var ev = new Event
                {
                    NameEvent= viewModel.NameEvent,
                    Teacher=viewModel.TeacherId,
                    Smena=viewModel.Smena,
                    NumLesson=viewModel.NumLesson,
                    Date = viewModel.Date,
                };
                await context.Events.AddAsync(ev);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("IndexDirector", "Timetable");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RegStudent()
        {
            var viewModel = new AddStudentViewModel();
            //var users = await timetable.GetUsersAsync();
            var genders=await timetable.GetGendersAsync();
            var classes=await timetable.GetClassesAsync();
            var users1 = context.Users.Where(e1 => !context.Students.Any(e2 => e2.UserI == e1.UserId))
                .Where(e1 => !context.Teachers.Any(e3 => e3.Userl == e1.UserId))
                .Where(e1 => !context.Directors.Any(e4 => e4.UserI == e1.UserId)).ToList();

            var items = users1.Select(c =>
            new SelectListItem {Text=c.Login, Value=c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text=c.GenderName, Value=c.GenderId.ToString()});

            var itemsTwo = classes.Select(c =>
            new SelectListItem { Text=c.NameClass, Value=c.ClassId.ToString()});

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            viewModel.Classes.AddRange(itemsTwo);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegStudentAsync(AddStudentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                var student = new Student
                {
                    Name = viewModel.Name,
                    LastName = viewModel.LastName,
                    SurName = viewModel.SurName,
                    Class=viewModel.ClassId,
                    Gender=viewModel.GenderId,
                    UserI=viewModel.UserId,
                    Birth=viewModel.Birth,
                    Image=null
                };
                await context.Students.AddAsync(student);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("Adding","Timetable");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RegTeacher()
        {
            var viewModel = new AddTeacherViewModel();
            //var users = await timetable.GetUsersAsync();
            var genders = await timetable.GetGendersAsync();
            var users1 = context.Users.Where(e1 => !context.Students.Any(e2 => e2.UserI == e1.UserId))
                .Where(e1 => !context.Teachers.Any(e3 => e3.Userl == e1.UserId))
                .Where(e1 => !context.Directors.Any(e4 => e4.UserI == e1.UserId)).ToList();

            var items = users1.Select(c =>
            new SelectListItem { Text = c.Login, Value = c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text = c.GenderName, Value = c.GenderId.ToString() });

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegTeacher(AddTeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                var teacher = new Teacher
                {
                    TeacherName = viewModel.Name,
                    TeacherLastName = viewModel.LastName,
                    TeacherSurName = viewModel.SurName,
                    Gender = viewModel.GenderId,
                    Userl = viewModel.UserId,
                    Birth = viewModel.Birth,
                    Image = null
                };
                await context.Teachers.AddAsync(teacher);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("Adding", "Timetable");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RegDirector()
        {
            var viewModel = new AddDirectorViewModel();
            //var users = await timetable.GetUsersAsync();
            var genders = await timetable.GetGendersAsync();
            var statuses = await timetable.GetStatusesAsync();
            var users1 = context.Users.Where(e1 => !context.Students.Any(e2 => e2.UserI == e1.UserId))
                .Where(e1 => !context.Teachers.Any(e3 => e3.Userl == e1.UserId))
                .Where(e1 => !context.Directors.Any(e4 => e4.UserI == e1.UserId)).ToList();

            var items = users1.Select(c =>
            new SelectListItem { Text = c.Login, Value = c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text = c.GenderName, Value = c.GenderId.ToString() });

            var itemsTwo = statuses.Select(c =>
            new SelectListItem { Text = c.StatusName, Value = c.StatusId.ToString() });

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            viewModel.Statuses.AddRange(itemsTwo);
            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegDirector(AddDirectorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                var director = new Director
                {
                    DirectorName = viewModel.Name,
                    DirectorLastName = viewModel.LastName,
                    DirectorSurName = viewModel.SurName,
                    Status = viewModel.StatusId,
                    Gender = viewModel.GenderId,
                    UserI = viewModel.UserId,
                    Birth = viewModel.Birth,
                    Image = null
                };
                await context.Directors.AddAsync(director);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("Adding", "Timetable");
        }


        public async Task<IActionResult> SpisokStudents()
        {
            var viewModel = new TimetableViewModel
            {
                Teacher = await timetable.GetTeacherAsync(),
                Students = await timetable.GetStudentsAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> SpisokTeachers()
        {
            var viewModel = new TimetableViewModel
            {
                Teacher = await timetable.GetTeacherAsync(),
                Teachers=await timetable.GetTeacherssAsync(),
            };

            return View(viewModel);
        }
        public async Task<IActionResult> SpisokDirectors()
        {
            var viewModel = new TimetableViewModel
            {
                Teacher = await timetable.GetTeacherAsync(),
                Directors=await timetable.GetDirectorsAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeStudent(int studentId)
        {
            var student = await timetable.FindStudentAsync(studentId);
            if (student is null)
            {
                return NotFound();
            }
            var viewModel = new UpdateStudentViewModel
            {
                Id=student.StudentId,
                Name=student.Name,
                LastName=student.LastName,
                SurName=student.SurName,
                Birth=student.Birth,
                GenderId=student.Gender,
                UserId=student.UserI,
                ClassId=student.Class
            };
            var users = await timetable.GetUsersAsync();
            var genders = await timetable.GetGendersAsync();
            var classes = await timetable.GetClassesAsync();

            var items = users.Select(c =>
            new SelectListItem { Text = c.Login, Value = c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text = c.GenderName, Value = c.GenderId.ToString() });

            var itemsTwo = classes.Select(c =>
            new SelectListItem { Text = c.NameClass, Value = c.ClassId.ToString() });

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            viewModel.Classes.AddRange(itemsTwo);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStudent(UpdateStudentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var student = await timetable.FindStudentAsync(viewModel.Id);
            if (student is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден!");
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                student.StudentId = viewModel.Id;
                student.Name = viewModel.Name;
                student.SurName = viewModel.SurName;
                student.LastName = viewModel.LastName;
                student.Birth = viewModel.Birth;
                student.Gender = viewModel.GenderId;
                student.Class = viewModel.ClassId;
                student.UserI = viewModel.UserId;
                context.Students.Update(student);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("SpisokStudents","Timetable");
        }




        [HttpGet]
        public async Task<IActionResult> ChangeTeacher(int teacherId)
        {
            var teacher = await timetable.FindTeacherAsync(teacherId);
            if (teacher is null)
            {
                return NotFound();
            }
            var viewModel = new UpdateTeacherViewModel
            {
                Id=teacher.TeacherId,
                GenderId=teacher.Gender,
                UserId=teacher.Userl,
                Name=teacher.TeacherName,
                LastName=teacher.TeacherLastName,
                SurName=teacher.TeacherSurName,
                Birth=teacher.Birth
            };
            var users = await timetable.GetUsersAsync();
            var genders = await timetable.GetGendersAsync();

            var items = users.Select(c =>
            new SelectListItem { Text = c.Login, Value = c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text = c.GenderName, Value = c.GenderId.ToString() });

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeTeacher(UpdateTeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var teacher = await timetable.FindTeacherAsync(viewModel.Id);
            if (teacher is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден!");
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                teacher.TeacherId = viewModel.Id;
                teacher.TeacherName = viewModel.Name;
                teacher.TeacherSurName = viewModel.SurName;
                teacher.TeacherLastName = viewModel.LastName;
                teacher.Birth = viewModel.Birth;
                teacher.Gender = viewModel.GenderId;
                teacher.Userl = viewModel.UserId;
                context.Teachers.Update(teacher);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("SpisokTeachers", "Timetable");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeDirector(int directorId)
        {
            var director = await timetable.FindDirectorAsync(directorId);
            if (director is null)
            {
                return NotFound();
            }
            var viewModel = new UpdateDirectorViewModel
            {
                Id=director.DirectorId,
                Name=director.DirectorName,
                LastName=director.DirectorLastName,
                SurName=director.DirectorSurName,
                Birth=director.Birth,
                GenderId=director.Gender,
                UserId=director.UserI,
                StatusId=director.Status
            };
            var users = await timetable.GetUsersAsync();
            var genders = await timetable.GetGendersAsync();
            var statuses = await timetable.GetStatusesAsync();

            var items = users.Select(c =>
            new SelectListItem { Text = c.Login, Value = c.UserId.ToString() });

            var itemsOne = genders.Select(c =>
            new SelectListItem { Text = c.GenderName, Value = c.GenderId.ToString() });

            var itemsTwo = statuses.Select(c =>
            new SelectListItem { Text = c.StatusName, Value = c.StatusId.ToString() });

            viewModel.Users.AddRange(items);
            viewModel.Genders.AddRange(itemsOne);
            viewModel.Statuses.AddRange(itemsTwo);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDirector(UpdateDirectorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var director = await timetable.FindDirectorAsync(viewModel.Id);
            if (director is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден!");
                return View(viewModel);
            }
            DateOnly test = viewModel.Birth;
            if (test >= DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            try
            {
                director.DirectorId = viewModel.Id;
                director.DirectorName = viewModel.Name;
                director.DirectorSurName = viewModel.SurName;
                director.DirectorLastName = viewModel.LastName;
                director.Birth = viewModel.Birth;
                director.Gender = viewModel.GenderId;
                director.Status = viewModel.StatusId;
                director.UserI = viewModel.UserId;
                context.Directors.Update(director);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("SpisokDirectors", "Timetable");
        }

        public async Task<IActionResult> SpisokEvents()
        {
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(0);
            }
            var viewModel = new TimetableViewModel
            {
                Director = await timetable.GetDirectorAsync(),
                Events=await timetable.GetAllEventsAsync(),
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEvent(int eventId)
        {
            var ev = await timetable.FindEventAsync(eventId);
            if (ev is null)
            {
                return NotFound();
            }
            var viewModel = new UpdateEventViewModel
            {
                Id = ev.EventId,
                NameEvent = ev.NameEvent,
                Date = ev.Date,
                NumLesson = ev.NumLesson,
                Smena = ev.Smena,
                TeacherId = ev.Teacher,
            };
            var teachers = await timetable.GetTeachersAsync();

            var items = teachers.Select(c =>
            new SelectListItem { Text = c.TeacherName + " " + c.TeacherLastName + " " + c.TeacherSurName, Value = c.TeacherId.ToString() });

            viewModel.Teachers.AddRange(items);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEvent(UpdateEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var ev = await timetable.FindEventAsync(viewModel.Id);
            if (ev is null)
            {
                ModelState.AddModelError("not_found", "Ивент не найден!");
                return View(viewModel);
            }
            DateOnly test = viewModel.Date;
            if (test < DateOnly.Parse(DateTime.Today.ToShortDateString()))
            {
                ModelState.AddModelError("not_date", "Дата не подохдит");
                return View(viewModel);
            }
            bool exist = context.Events.Any(z => z.NameEvent == viewModel.NameEvent && z.NumLesson == viewModel.NumLesson && z.Smena == viewModel.Smena && z.Teacher == viewModel.TeacherId
            && z.Date == viewModel.Date);
            if (exist)
            {
                ModelState.AddModelError("exist", "уже существует, поэтому нет необходимости создавать новый элемент, который портит базу данных");
                return View(viewModel);
            }
            try
            {
                ev.EventId = viewModel.Id;
                ev.NameEvent = viewModel.NameEvent;
                ev.NumLesson = viewModel.NumLesson;
                ev.Smena = viewModel.Smena;
                ev.Date = viewModel.Date;
                ev.Teacher = viewModel.TeacherId;
                context.Events.Update(ev);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при сохранении в базу данных.");
                return View(viewModel);
            }
            return RedirectToAction("SpisokEvents", "Timetable");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var ev=await timetable.FindEventAsync(eventId);
            if (ev is null)
            {
                return NotFound();
            }
            var viewModel = new DeleteEventViewModel
            {
                Id=ev.EventId,
                NameEvent=ev.NameEvent,
                NumLesson=ev.NumLesson,
                Smena=ev.Smena,
                Date=ev.Date.ToString(),
                TeacherId=ev.Teacher
            };
            var teacher = await timetable.GetTeacherssAsync();
            viewModel.Teacher = teacher.First(z=>z.TeacherId==viewModel.TeacherId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(DeleteEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var ev = await timetable.FindEventAsync(viewModel.Id);
            if (ev is null)
            {
                ModelState.AddModelError("not_found", "Ивент не найден");
                return View(ev);
            }
            try
            {
                context.Events.Remove(ev);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при удалении из базы данных");
                return View(viewModel);
            }
            return RedirectToAction("SpisokEvents", "Timetable");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDirector(int directorId)
        {
            var director = await timetable.FindDirectorAsync(directorId);
            if (director is null)
            {
                return NotFound();
            }
            var viewModel = new DeleteDirectorViewModel
            {
                Id = director.DirectorId,
                Name = director.DirectorName,
                LastName = director.DirectorLastName,
                SurName = director.DirectorSurName,
                Birth = director.Birth.ToString(),
                GenderId = director.Gender,
                UserId = director.UserI,
                StatusId = director.Status
            };
            var genders = await timetable.GetGendersAsync();
            var users = await timetable.GetUsersAsync();
            var statuses=await timetable.GetStatusesAsync();
            viewModel.Gender = genders.First(z => z.GenderId == viewModel.GenderId);
            viewModel.User = users.First(z => z.UserId == viewModel.UserId);
            viewModel.Status=statuses.First(z=>z.StatusId == viewModel.StatusId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirector(DeleteDirectorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var director = await timetable.FindDirectorAsync(viewModel.Id);
            if (director is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден");
                return View(director);
            }
            try
            {
                context.Directors.Remove(director);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при удалении из базы данных");
                return View(viewModel);
            }
            return RedirectToAction("SpisokDirectors", "Timetable");
        }




        [HttpGet]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await timetable.FindStudentAsync(studentId);
            if (student is null)
            {
                return NotFound();
            }
            var viewModel = new DeleteStudentViewModel
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                SurName = student.SurName,
                Birth = student.Birth.ToString(),
                GenderId = student.Gender,
                UserId = student.UserI,
                ClassId = student.Class
            };
            var genders = await timetable.GetGendersAsync();
            var users = await timetable.GetUsersAsync();
            var classes = await timetable.GetClassesAsync();
            viewModel.Gender = genders.First(z => z.GenderId == viewModel.GenderId);
            viewModel.User = users.First(z => z.UserId == viewModel.UserId);
            viewModel.Class = classes.First(z => z.ClassId == viewModel.ClassId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStudent(DeleteStudentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var student = await timetable.FindStudentAsync(viewModel.Id);
            if (student is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден");
                return View(student);
            }
            List<StudentParent> studentParent = context.StudentParents.Where(z => z.Student == student.StudentId).ToList();
            try
            {
                foreach (StudentParent parent in  studentParent)
                {
                    context.StudentParents.Remove(parent);
                }
                context.Students.Remove(student);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при удалении из базы данных");
                return View(viewModel);
            }
            return RedirectToAction("SpisokStudents", "Timetable");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTeacher(int teacherId)
        {
            var teacher = await timetable.FindTeacherAsync(teacherId);
            if (teacher is null)
            {
                return NotFound();
            }
            var viewModel = new DeleteTeacherViewModel
            {
                Id = teacher.TeacherId,
                Name = teacher.TeacherName,
                LastName = teacher.TeacherLastName,
                SurName = teacher.TeacherSurName,
                Birth = teacher.Birth.ToString(),
                GenderId = teacher.Gender,
                UserId = teacher.Userl
            };
            var genders = await timetable.GetGendersAsync();
            var users = await timetable.GetUsersAsync();
            viewModel.Gender = genders.First(z => z.GenderId == viewModel.GenderId);
            viewModel.User = users.First(z => z.UserId == viewModel.UserId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeacher(DeleteTeacherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var teacher = await timetable.FindTeacherAsync(viewModel.Id);
            if (teacher is null)
            {
                ModelState.AddModelError("not_found", "Чел не найден");
                return View(teacher);
            }
            List<Event> events=context.Events.Where(z=>z.Teacher==teacher.TeacherId).ToList();
            List<Class> classes = context.Classes.Where(z => z.ClassRuk == teacher.TeacherId).ToList();
            List<TeacherSubject> teacherSubjects = context.TeacherSubjects.Where(z => z.Teacher == teacher.TeacherId).ToList();
            try
            {
                
                foreach (Event ev in events)
                {
                    context.Events.Remove(ev);
                }
                foreach (TeacherSubject ts in teacherSubjects)
                {
                    List<TimeTable> tb=context.TimeTables.Where(z=>z.TeacherSubject == ts.TeacherSubjectId).ToList();
                    foreach(TimeTable tt in tb)
                    {
                        context.TimeTables.Remove(tt);
                    }
                    context.TeacherSubjects.Remove(ts);
                }
                foreach (Class cl in classes)
                {
                    List<TimeTable> tb=context.TimeTables.Where(z=>z.Class==cl.ClassId).ToList();
                    List<Student> st=context.Students.Where(z=>z.Class == cl.ClassId).ToList();
                    foreach (TimeTable tt in tb)
                    {
                        context.TimeTables.Remove(tt);
                    }
                    foreach (Student dd in st)
                    {
                        List<StudentParent> stp=context.StudentParents.Where(z=>z.Student==dd.StudentId).ToList();
                        foreach (StudentParent sss in stp)
                        {
                            context.StudentParents.Remove(sss);
                        }
                        context.Students.Remove(dd);
                    }
                    context.Classes.Remove(cl);
                }
                context.Teachers.Remove(teacher);
                context.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("database", "Ошибка при удалении из базы данных");
                return View(viewModel);
            }
            return RedirectToAction("SpisokTeachers", "Timetable");
        }
    }
}

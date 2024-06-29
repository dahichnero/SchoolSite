using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchkalkaB.Data;
using SchkalkaB.Domain.Services;
using SchkalkaB.Infrastructure;
using SchkalkaB.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.ExpireTimeSpan=TimeSpan.FromHours(1);
        opt.Cookie.Name = "timetable_session";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.LoginPath = "/User/Login";
    });
builder.Services.AddDbContext<SchkalkaDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("local")));
builder.Services.AddScoped<IRepository<User>, EFRepository<User>>();
builder.Services.AddScoped<IRepository<Role>, EFRepository<Role>>();
builder.Services.AddScoped<IUserInterface,UserService>();
builder.Services.AddScoped<IRepository<TimeTable>, EFRepository<TimeTable>>();
builder.Services.AddScoped<IRepository<Student>, EFRepository<Student>>();
builder.Services.AddScoped<IRepository<Class>, EFRepository<Class>>();
builder.Services.AddScoped<IRepository<Parent>, EFRepository<Parent>>();
builder.Services.AddScoped<IRepository<Teacher>, EFRepository<Teacher>>();
builder.Services.AddScoped<IRepository<Event>, EFRepository<Event>>();
builder.Services.AddScoped<IRepository<Gender>, EFRepository<Gender>>();
builder.Services.AddScoped<IRepository<Status>, EFRepository<Status>>();
builder.Services.AddScoped<IRepository<Director>, EFRepository<Director>>();
builder.Services.AddScoped<ITimetable, TimeTableSearch>();
builder.Services.AddScoped<ITimetableService,TimetableService>();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{Controller=Timetable}/{Action=Index}");
app.Run();

using SchkalkaB.Models;

namespace SchkalkaB.Domain.Services
{
    public interface IUserInterface
    {
        Task<bool> IsUserExistsAsync(string username);

        Task<User> RegistrationAsync(string username, string password);
        Task<Student> RegistrationSAsync(string username, string fullname, string password);
        Task<Director> RegistrationDAsync(string username, string fullname, string password);
        Task<Teacher> RegistrationTAsync(string username, string fullname, string password);
        Task<User?> GetUserAsync(string username, string password);
    }
}

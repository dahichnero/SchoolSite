using Azure.Identity;
using SchkalkaB.Domain.Services;
using SchkalkaB.Models;
using System.Security.Cryptography;
using System.Text;

namespace SchkalkaB.Infrastructure
{
    public class UserService : IUserInterface
    {
        private readonly IRepository<User> users;
        private readonly IRepository<Role> roles;

        public UserService(IRepository<User> users, IRepository<Role> roles)
        {
            this.users = users;
            this.roles = roles;
        }

        private string GetSalt()
        {
            return DateTime.UtcNow.ToString() + DateTime.Now.Ticks;
        }

        private string GetSha256(string password, string salt)
        {
            byte[] passwordBytes=Encoding.UTF8.GetBytes(password+salt);
            byte[] hashBytes=SHA256.HashData(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
        public async Task<User?> GetUserAsync(string username, string password)
        {
            username=username.Trim();
            User? user = (await users.FindWhere(u => u.Login == username)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            //string hashPassword = GetSha256(password, user.Password);//соль
            if (user.Password != password)
            {
                return null;
            }
            return user;
        }

        public async Task<bool> IsUserExistsAsync(string username)
        {
            username = username.Trim();
            User? found=(await users.FindWhere(u=>u.Login==username)).FirstOrDefault();
            return found != null;
        }

        public Task<Director> RegistrationDAsync(string username, string fullname, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Student> RegistrationSAsync(string username, string fullname, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher> RegistrationTAsync(string username, string fullname, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<User> RegistrationAsync(string username, string password)
        {
            bool userExist = await IsUserExistsAsync(username);
            if (userExist)
            {
                throw new ArgumentException("UserName already exist");
            }
            Role? userrole=(await roles.FindWhere(r=>r.NameRole=="user")).FirstOrDefault();
            if (userrole is null)
            {
                throw new InvalidOperationException("Role 'client' not found in database");
            }
            User toAdd = new User
            {
                Login = username,
                Role=userrole.RoleId,
                Password=password
            };
            return await users.AddAsync(toAdd);
        }
    }
}

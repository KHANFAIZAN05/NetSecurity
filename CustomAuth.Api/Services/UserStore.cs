using CustomAuth.Api.Models;

namespace CustomAuth.Api.Services
{
    public class UserStore
    {
        private List<User> _users = new()
        {
            new User { UserName = "admin", Password = "admin123", Role = "Admin" },
            new User { UserName = "user", Password = "user123", Role = "User" }
        };

        public User? ValidateUser(string username, string password)
        {
            return _users.FirstOrDefault(u =>
                u.UserName == username && u.Password == password);
        }
    }

}

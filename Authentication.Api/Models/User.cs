namespace Authentication.Api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public  string? PasswordHash { get; set; }
        public required string Role { get; set; }
    }
}

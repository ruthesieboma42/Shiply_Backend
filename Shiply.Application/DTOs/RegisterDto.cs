
namespace Shiply.Application.DTOs
{
    
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; } 

        
        public string? Address { get; set; }       
        public string? LicenseNumber { get; set; } 
    }
}

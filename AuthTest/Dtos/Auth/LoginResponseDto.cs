using DataAccess.Entities;

namespace AuthTest.Dtos.Auth
{
    public class LoginResponseDto 
    {
        
        public int Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }  
        public string Jwt { get; set; }
    }
}

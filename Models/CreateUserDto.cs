using System.ComponentModel.DataAnnotations;

namespace IlmHub04.Models
{
    public class CreateUserDto
    {
        [StringLength(50), MinLength(3)]
        public string Firstname { get; set; }
        public string Lastname { get; set; }



        [StringLength(50), MinLength(3)]
        public string Username { get; set; }
        [StringLength(50), MinLength(6)]
        public string Password { get; set; }
        public IFormFile Photo { get; set; }
        
    }
}

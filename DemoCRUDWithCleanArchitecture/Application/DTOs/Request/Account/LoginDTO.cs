using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Account
{
    public class LoginDTO
    {
        [EmailAddress,Required,DataType(DataType.EmailAddress)]
        [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+",
            ErrorMessage ="Your email is not valid, provide valid email such ass ...@gmail, @hostmail, etc...")]
       [DisplayName("Email Address")] public string EmailAddress { get; set; } = string.Empty;

        [Required,DataType(DataType.Password)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*_]).{8,}$",
            ErrorMessage ="Yor password must be a mix of Alphanumeric and special characters")]
        public string Password { get; set; } = string.Empty;
    }
}

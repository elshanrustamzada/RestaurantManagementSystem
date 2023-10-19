using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Bos ola bilmez!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }

        public bool IsRemember { get; set; }
    }
}

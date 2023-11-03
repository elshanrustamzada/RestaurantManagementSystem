using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.ViewModels
{
    public class ResetPasswordVM
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string CheckPassword { get; set; }
       
    }
}

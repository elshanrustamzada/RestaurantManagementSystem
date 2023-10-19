using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Bos ola bilmez")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bos ola bilmez")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}

using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    public class Employer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public long? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public string Salary { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public bool IsDeactive { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz!")]
        public DateTime? Birthday { get; set; }
        //public string GetFormattedBirthday()
        //{
        //    return Birthday?.ToString("dd:mm:yyyy");
        //}
        //public string GetFormattedBirthday()
        //{
        //    // "ToShortDateString()" metodu, tarihi kısa tarih biçiminde döndürür (örn. "20.07.1993")
        //    return Birthday?.ToShortDateString("dd:mm:yyyy");
        //}



        [NotMapped]
        public IFormFile? Photo { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
    }
}

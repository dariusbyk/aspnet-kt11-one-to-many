using System.ComponentModel.DataAnnotations;
namespace LibraryApp.Models
{
    public class AuthorForm
    {
        [Required(ErrorMessage = "Введите имя автора.")]
        public string Name { get; set; }
    }
}

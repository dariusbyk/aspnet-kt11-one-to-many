using System.ComponentModel.DataAnnotations;
namespace LibraryApp.Models
{
    public class BookForm
    {
        [Required(ErrorMessage = "Введите название книги.")]
        public string Title { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Выберите автора.")]
        public int AuthorId { get; set; }
    }
}

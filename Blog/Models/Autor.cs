using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Autor
    {
        public int AutorId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100)]
        public string? Nome { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}

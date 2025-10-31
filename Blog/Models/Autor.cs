using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Autor
    {
        public int AutorId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 100 caracteres.")]
        [Display(Name = "Nome Completo")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do Email é inválido.")]
        [StringLength(150, ErrorMessage = "O Email deve ter no máximo 150 caracteres.")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    }
}

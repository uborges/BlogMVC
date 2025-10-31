using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Título deve ter no máximo 100 caracteres.")]
        [Display(Name = "Título do Post")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "O campo Conteúdo é obrigatório.")]
        [Column(TypeName = "ntext")] // Sugerindo ntext para conteúdo maior
        [Display(Name = "Conteúdo")]
        public string? Conteudo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // chave estrangeira para Autor
        [Required(ErrorMessage = "O campo Autor é obrigatório.")]
        [Display(Name = "Autor")]
        public int AutorId { get; set; }

        // um post pertence a um autor
        public virtual Autor? Autor { get; set; }
    }
}

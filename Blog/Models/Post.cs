using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Titulo { get; set; }
        public string? Conteudo { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // chave estrangeira para Autor
        public int AutorId { get; set; }

        // um post pertence a um autor
        public virtual Autor? Autor { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.Model
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string ComentarioLivro { get; set; }

        [Required]
        public int LivroId { get; set; }

        public string DataInsercao { get; set; }
    }
}
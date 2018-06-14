using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.Model
{
    public class Livro
    {
        public int Id { get; set; }

        [Required]
        public string Registro { get; set; }     

        [Required]
        [StringLength(256)]
        public string Titulo { get; set; }

        [Required]
        public string Autor { get; set; }

        public string Ano { get; set; }

        [Required]
        public double Valor { get; set; }
    }
}
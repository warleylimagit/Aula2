using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.ViewModel
{
    public class CarrinhoLivroViewModel
    {
        [Required]
        public int IdLivro { get; set; }
    }
}
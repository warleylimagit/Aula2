using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace LivroApi.Model
{
    public class Carrinho 
    {
        public int Id { get; set; }

        public ICollection<Livro> Livros { get; set; }

        public double ValorTotal { get; set; }
    }
}
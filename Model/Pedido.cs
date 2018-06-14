using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.Model
{
    public class Pedido
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public Carrinho Carrinho { get; set; }
    }
}
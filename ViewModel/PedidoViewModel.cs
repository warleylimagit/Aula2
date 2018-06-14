using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.ViewModel
{
    public class PedidoViewModel
    {
        [Required]
        public int IdCarrinho { get; set; }
    }
}
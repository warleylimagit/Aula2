using System;
using System.ComponentModel.DataAnnotations;

namespace LivroApi.Model
{
    public class Cartao
    {
        [Required]
        public string BandeiraCartao { get; set; }

        [Required]
        public string UsuarioCartao { get; set; }

        [Required]
        public string NumeroCartao { get; set; }

        [Required]
        public string CodigoCartao { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public int Parcelas { get; set; }

        [Required]
        public string TokenUsuario { get; set; }
    }
}
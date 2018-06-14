using System;
using System.Collections.Generic;
using System.Linq;
using LivroApi.Model;

namespace LivroApi.Repositorio
{
    public static class RepositorioDados
    {
        public static IList<Livro> GetLivros()
        {
            List<Livro> _livroList = new List<Livro>();
            for (int i = 0; i < 5; i++)
            {
                Livro _livro = new Livro{
                Id = i,
                Ano = "2018",
                Autor = $"Autor{i}",
                Registro = $"f12345{i}",
                Titulo = $"Titulo{i}",
                Valor = i + 10
                };

                _livroList.Add(_livro);
            }

            return _livroList;
        }

        public static Carrinho GetCarrinho()
        {
            Carrinho _carrinho = new Carrinho();

            _carrinho.Id = 1;

            _carrinho.Livros = new List<Livro>();

            for (int i =0; i < 5; i++)
            {
                Livro _livro = new Livro
                {
                    Id = i,
                    Ano = "2018",
                    Autor = $"Autor{i}",
                    Registro = $"f12345{i}",
                    Titulo = $"Titulo{i}",
                    Valor = i + 10
                };
                
                _carrinho.Livros.Add(_livro);
            }

            _carrinho.ValorTotal = _carrinho.Livros.Sum(l => l.Valor);
           
            return _carrinho;
        }

        public static List<Pedido> GetPedidos()
        {            
            List<Pedido> _pedidoList = new List<Pedido>();

            for (int i = 0; i < 3; i++)
            {
                var _livros = GetLivros().Where(l => l.Id == i || l.Id == i+1);
                
                Carrinho _carrinho = new Carrinho
                {                    
                    Id = (i+1),
                    Livros = _livros.ToList(),
                    ValorTotal = _livros.Sum(l => l.Valor)
                };

                Pedido _pedido = new Pedido
                {
                    Id = i+1,
                    Carrinho = _carrinho,
                    Status = i == 0 ? "Fechado" : "Aberto"
                };

                _pedidoList.Add(_pedido);
            }     

            return _pedidoList;     
        } 
    }
}
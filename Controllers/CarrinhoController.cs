using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LivroApi.Model;
using System.Net;
using LivroApi.ViewModel;
using LivroApi.Repositorio;

namespace LivroApi.Controllers
{
    [Route("api/v1/[controller]")]   
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RepositorioDados.GetCarrinho());                        
        }
        
        [HttpGet("{id}")]
        public IActionResult Livro(int id)
        {
            var _carrinho = RepositorioDados.GetCarrinho(); 

            _carrinho.Livros = _carrinho.Livros.Where(l => l.Id == id ).ToList();

            if(_carrinho.Livros == null || _carrinho.Livros.Count == 0)
                return NotFound();
            
            _carrinho.ValorTotal = _carrinho.Livros.Sum(l => l.Valor);

            return StatusCode(302, _carrinho);
        } 

        [HttpPost]
        public IActionResult Post([FromBody] CarrinhoLivroViewModel viewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                List<Livro> _livro = new List<Livro>();
                
                var livros = RepositorioDados.GetLivros();

                var livro = livros.Where(l => l.Id == viewModel.IdLivro).SingleOrDefault();

                if(livro == null)
                    return NotFound();    
                else
                    _livro.Add(livro);

                Carrinho _carrinho = new Carrinho
                {
                    Id = 1,
                    Livros = _livro
                };

                _carrinho.ValorTotal = _carrinho.Livros.Sum(l => l.Valor);

                return Created("", _carrinho);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            try
            {
                if(!id.HasValue)
                    return BadRequest("Necessario o Id do livro para excluir do carrinho!");
                
                if(!RepositorioDados.GetCarrinho().Livros.Any(l => l.Id == id))
                    return NotFound();

                return Ok("Sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
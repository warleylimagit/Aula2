using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LivroApi.Model;
using LivroApi.Repositorio;
using System.Net;

namespace LivroApi.Controllers
{
    [Route("api/v1/[controller]")]    
    [ApiController]
    public class LivroController : ControllerBase
    {    
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RepositorioDados.GetLivros()); 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {   
            var _livrosList = RepositorioDados.GetLivros();

            Livro livro = _livrosList.Where(l => l.Id == id).FirstOrDefault();

            if(livro == null)
                return NotFound("Livro não encontrado!");

            return StatusCode(302, livro); //Retorna status Found
        }

        [Route("api/v1/[controller]/[action]")]
        [HttpGet]
        public IActionResult GetLivro([FromQuery] Livro livro)
        {
            if(livro == null)
                return BadRequest("Necessário informar pelo menos 1 item para buscar o livro!");
            
            var _livro = RepositorioDados.GetLivros()
                                .Where(l => l.Ano == livro.Ano || l.Autor.Contains(livro.Autor) 
                                || l.Registro == livro.Registro || l.Titulo.Contains(livro.Titulo)
                                || l.Valor == livro.Valor)
                                .ToList();
            
            if(_livro == null || _livro.Count == 0)
                return NotFound();

            return StatusCode(302, _livro);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Livro _livro)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Created($"api/vi/{_livro.Registro}", _livro);
            }
            catch (System.Exception)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }            
        }

        [HttpPut("{id}")]
        public IActionResult Put(int? id, [FromBody] Livro _livro)
        {
            if(!id.HasValue)
                return BadRequest("Necessário o Id do livro para atualizar os dados!");
            
            Livro livro = RepositorioDados.GetLivros()
                                          .Where(l => l.Id == id).FirstOrDefault();
            if(livro == null)
                return NotFound(livro);

            livro.Ano = _livro.Ano;
            livro.Autor = _livro.Autor;
            livro.Registro = _livro.Registro;
            livro.Titulo = _livro.Titulo;

            return Ok(livro);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if(!id.HasValue)
                return BadRequest("Necessário o Id para excluir o Livro!");
            
            Livro livro = RepositorioDados.GetLivros()
                                          .Where(l => l.Id == id).FirstOrDefault();
            if(livro == null)
                return NotFound();

            return Ok("Sucesso");
        }
        
        [Route("api/v1/[controller]/[action]")]
        [HttpPost]
        public IActionResult Comentario([FromBody] Comentario comentario)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!RepositorioDados.GetLivros().Any(l => l.Id == comentario.LivroId))
                return NotFound();

            Comentario _comentario = new Comentario{
                Id = 3,
                ComentarioLivro = comentario.ComentarioLivro,
                LivroId = comentario.LivroId,
                DataInsercao = DateTime.Now.Date.ToString("yyy/MM/dd")
            };   
            
            return Created("", _comentario);
        }
    }
}
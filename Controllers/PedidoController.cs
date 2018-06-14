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
    [ApiController]
    public class PedidoController : ControllerBase
    {        
        [HttpGet("api/v1/[controller]")]
        public IActionResult Get()
        {
            return Ok(RepositorioDados.GetPedidos());
        }
        
        [HttpGet("api/v1/[controller]/{id}")]
        public IActionResult Get(int id)
        {            
            Pedido _pedido = RepositorioDados.GetPedidos().Where(p => p.Id == id).FirstOrDefault();

            if(_pedido == null)
                return NotFound();
            
            return StatusCode(302, _pedido);
        }
        
        [HttpGet("api/v1/[controller]/[action]/{id}")]
        public IActionResult GetStatus(int id)
        {
            var _pedidos = RepositorioDados.GetPedidos();

            var _pedido = _pedidos.Where(p => p.Id == id)
                                  .Select(p => new {Status = p.Status, Id = p.Id})
                                  .FirstOrDefault();

            if(_pedido == null)
                return NotFound();            
            
            return StatusCode(302, _pedido);
        }        

        [HttpPost("api/v1/[controller]")]
        public IActionResult Post([FromBody] PedidoViewModel viewModel)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                Carrinho _carrinho = RepositorioDados.GetCarrinho();
                _carrinho.Id = viewModel.IdCarrinho;

                Pedido _pedido = new Pedido
                {
                    Carrinho = _carrinho,
                    Id = 1,
                    Status = "Aberto"
                };

                return Created("", _pedido);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("api/v1/[controller]/{id}")]
        public IActionResult Put(int? id)
        {
            try
            {
                if(!id.HasValue)
                    return BadRequest("Necessário o Id para atualizar o status do pedido!");
                
                Pedido _pedido = RepositorioDados.GetPedidos().Where(p => p.Id == id).FirstOrDefault();
                
                if(_pedido == null)
                    return NotFound();
                
                _pedido.Status = "Fechado";

                return Ok(_pedido);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("api/v1/[controller]/{id}")]
        public IActionResult Delete(int? id)
        {
            try
            {
                if(!id.HasValue)
                    return BadRequest("Necessário o Id para excluir o pedido!");

                if(!RepositorioDados.GetPedidos().Any(p => p.Id == id))
                    return NotFound();

                return Ok("Sucesso");
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
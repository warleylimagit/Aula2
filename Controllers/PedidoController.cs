using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LivroApi.Model;
using System.Net;
using LivroApi.ViewModel;
using LivroApi.Repositorio;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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

        [HttpPost("{id}")]
        public IActionResult Pagamento(int id, [FromBody] Cartao _cartao)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                // if(!EfetuaPagamento(_cartao))
                //     return BadRequest("Operação não autorizada!");

                // return Ok("Sucesso!");
                return EfetuaPagamento(_cartao);

            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }
        }

        private async Task<bool> ValidaUsuario(string token)
        {
                using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => false; 
                HttpResponseMessage response = await client.GetAsync($"https://localhost:5050/api/v1/usuario/validatoken?_token={token}");

                if(response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }      
        }
        
        private IActionResult EfetuaPagamento(Cartao _cartao)
        {
            try
            {
                string conteudo;
                dynamic resultado;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    string _uri = $@"https://localhost:5051/api/v1/cartao?BandeiraCartao={_cartao.BandeiraCartao}&UsuarioCartao={_cartao.UsuarioCartao}
                    &NumeroCartao={_cartao.NumeroCartao}&CodigoCartao={_cartao.CodigoCartao}&Valor={_cartao.Valor}&Parcelas={_cartao.Parcelas}
                    &TokenUsuario={_cartao.TokenUsuario}";

                    //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => false; 
                    HttpResponseMessage response = client.GetAsync(_uri).Result;                

                    if(response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // conteudo = response.Content.ReadAsStringAsync().Result;
                        // resultado = JsonConvert.DeserializeObject(conteudo);

                        return Ok("Sucesso!");
                    }

                    conteudo = response.Content.ReadAsStringAsync().Result;
                    resultado = JsonConvert.DeserializeObject(conteudo);
                    return BadRequest(resultado);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }             
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LivroApi.Model;

namespace LivroApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        [HttpGet]
        public ActionResult <IEnumerable<Livro>> Get()
        {
            List<Livro> _livroList = new List<Livro>();
            for (int i = 0; i < 5; i++)
            {
                Livro _livro = new Livro{
                Ano = "2018",
                Autor = $"Autor{i}",
                Registro = $"f12345{i}",
                Titulo = $"Titulo{i}"
                };

                _livroList.Add(_livro);
            }

            return _livroList; 
        }
    }
}
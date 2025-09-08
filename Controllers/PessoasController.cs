using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SeniorAPITeste.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PessoasController : ControllerBase
    {
        private static List<Pessoa> _pessoas = new List<Pessoa>();
        private static int _nextId = 1;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_pessoas);
        }

        [HttpGet("{codigo}")]
        public IActionResult GetById(int codigo)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound(new { message = "Pessoa não encontrada" });

            return Ok(pessoa);
        }

        [HttpGet("uf/{uf}")]
        public IActionResult GetByUF(string uf)
        {
            var pessoas = _pessoas.Where(p => p.UF.Equals(uf, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(pessoas);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pessoa pessoa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            pessoa.Codigo = _nextId++;
            _pessoas.Add(pessoa);
            return CreatedAtAction(nameof(GetById), new { codigo = pessoa.Codigo }, pessoa);
        }

        [HttpPut("{codigo}")]
        public IActionResult Update(int codigo, [FromBody] Pessoa pessoa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (existingPessoa == null)
                return NotFound(new { message = "Pessoa não encontrada" });

            existingPessoa.Nome = pessoa.Nome;
            existingPessoa.CPF = pessoa.CPF;
            existingPessoa.UF = pessoa.UF;
            existingPessoa.DataNascimento = pessoa.DataNascimento;

            return Ok(existingPessoa);
        }

        [HttpDelete("{codigo}")]
        public IActionResult Delete(int codigo)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound(new { message = "Pessoa não encontrada" });

            _pessoas.Remove(pessoa);
            return NoContent();
        }
    }

    public class Pessoa
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        public required string CPF { get; set; }

        [Required(ErrorMessage = "UF é obrigatório")]
        [StringLength(2, ErrorMessage = "UF deve ter 2 caracteres")]
        public required string UF { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }
    }
}
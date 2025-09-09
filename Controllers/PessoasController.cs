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
        private static readonly List<Pessoa> _pessoas = new();
        private static int _nextId = 1;

        // Coleta erros de validação do ModelState para resposta
        private Dictionary<string, string[]> ErrorsFromModelState() =>
            ModelState
                .Where(kvp => kvp.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors
                                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage)
                                    ? "Erro de validação"
                                    : e.ErrorMessage)
                                .ToArray()
                );

        [HttpGet]
        public IActionResult GetAll()
        {
            var msg = _pessoas.Count == 0
                ? "Nenhuma pessoa cadastrada"
                : $"Total de pessoas: {_pessoas.Count}";
            return Ok(new ApiResponse<IEnumerable<Pessoa>>(msg, _pessoas));
        }

        [HttpGet("{codigo:int}")]
        public IActionResult GetById(int codigo)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound(new ApiError("Pessoa não encontrada"));

            return Ok(new ApiResponse<Pessoa>("Pessoa encontrada", pessoa));
        }

        [HttpGet("uf/{uf}")]
        public IActionResult GetByUF(string uf)
        {
            var lista = _pessoas
                .Where(p => p.UF.Equals(uf, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var msg = lista.Count == 0
                ? "Nenhuma pessoa para a UF informada"
                : $"Total na UF {uf.ToUpperInvariant()}: {lista.Count}";

            return Ok(new ApiResponse<IEnumerable<Pessoa>>(msg, lista));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pessoa pessoa)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationError("Dados inválidos", ErrorsFromModelState()));

            pessoa.Codigo = _nextId++;
            _pessoas.Add(pessoa);

            // 201 Created com Location apontando para GET /api/pessoas/{codigo}
            return CreatedAtAction(
                nameof(GetById),
                new { codigo = pessoa.Codigo },
                new ApiResponse<Pessoa>("Pessoa criada com sucesso", pessoa)
            );
        }

        [HttpPut("{codigo:int}")]
        public IActionResult Update(int codigo, [FromBody] Pessoa pessoa)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiValidationError("Dados inválidos", ErrorsFromModelState()));

            var existing = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (existing == null)
                return NotFound(new ApiError("Pessoa não encontrada"));

            existing.Nome = pessoa.Nome;
            existing.CPF = pessoa.CPF;
            existing.UF = pessoa.UF;
            existing.DataNascimento = pessoa.DataNascimento;

            return Ok(new ApiResponse<Pessoa>("Pessoa atualizada com sucesso", existing));
        }

        [HttpDelete("{codigo:int}")]
        public IActionResult Delete(int codigo)
        {
            var pessoa = _pessoas.FirstOrDefault(p => p.Codigo == codigo);
            if (pessoa == null)
                return NotFound(new ApiError("Pessoa não encontrada"));

            _pessoas.Remove(pessoa);

            // 200 OK para poder enviar mensagem (ao invés de 204 NoContent)
            return Ok(new ApiResponse<object>("Pessoa excluída com sucesso", new { codigo }));
        }
    }

    // Envelopes de resposta
    public record ApiResponse<T>(string Message, T Data);
    public record ApiError(string Message);
    public record ApiValidationError(string Message, Dictionary<string, string[]> Errors);

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
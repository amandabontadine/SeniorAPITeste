using SeniorAPITeste.Models;

namespace SeniorAPITeste.Repositories;

public interface IPessoaRepository
{
    IEnumerable<Pessoa> Listar();
    IEnumerable<Pessoa> ListarPorUf(string uf);
    Pessoa? Obter(int codigo);
    Pessoa Adicionar(Pessoa pessoa);
    Pessoa? Atualizar(int codigo, Pessoa pessoa);
    bool Remover(int codigo);
}